using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code;
using Core;
using Tech;
using UI;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance 
        => _instance 
            ? _instance 
            : _instance = FindObjectOfType<GameManager>();
    #endregion
    
    private WaypointProvider _waypointProvider;
    public WaypointProvider WaypointProvider
        => _waypointProvider 
            ? _waypointProvider 
            : _waypointProvider = FindObjectOfType<WaypointProvider>();

    private MeetingRoomInteractible _meetingRoomInteractible;
    public MeetingRoomInteractible MeetingRoomInteractible
        => _meetingRoomInteractible
            ? _meetingRoomInteractible
            : _meetingRoomInteractible = FindObjectOfType<MeetingRoomInteractible>();
    
    private EffectController _effectController;
    public EffectController EffectController
        => _effectController
            ? _effectController
            : _effectController = FindObjectOfType<EffectController>();

    private NotificationController _notificationController;

    public NotificationController NotificationController
        => _notificationController
            ? _notificationController
            : _notificationController = FindObjectOfType<NotificationController>();

    private GameSpeedController _gameSpeedController;
    public GameSpeedController GameSpeedController
        => _gameSpeedController
            ? _gameSpeedController
            : _gameSpeedController = FindObjectOfType<GameSpeedController>();

    private SongHandler _songHandler;
    public SongHandler SongHandler
        => _songHandler
            ? _songHandler
            : _songHandler = FindObjectOfType<SongHandler>();
    
    private Clock _clock;
    public Clock Clock => _clock ? _clock : _clock = FindObjectOfType<Clock>();
    
    [ReadOnly] public Player player;

    public GameState GameState = GameState.INIT;

    public InteractibleManager InteractibleManager;

    public int Day = 0;

    public bool ScrumMasterActive = false;
    
    public Company Company { get; private set; }
    [SerializeField] private float _impedimentChance = 0.1f;

    private void Start()
    {
        InitPlayerAvatar();
        InitCompany();
        InitAlarms();
        
        InitNewDay();
    }

    private void InitAlarms()
    {
        Clock.SetAlarm(new TimeStamp(16,00,0), WarnFinishDay, true);
        Clock.SetAlarm(new TimeStamp(17,00,0), FinishDay, true);
    }

    private void InitCompany()
    {
        Company = new Company(name);

        FindObjectsOfType<NPC>().ToList().ForEach(entity =>
        {
            Company.RegisterTeamMember(entity);
        });
    }

    private void WarnFinishDay()
    {
        Clock.SetToWarningColor();
        NotificationController.DisplayNotification("Der Arbeitstag ist in einer Stunde vorbei!", NotificationType.Warning);
    }
    
    private void FinishDay()
    {
        GameState = GameState.ADVISE;
        Clock.Running = false;
        Clock.SetToBaseColor();
        
        InteractibleManager.Phone.StopRinging();
        UiManager.Instance.HideAllScreens();
        UiManager.Instance.ShowScoreScreen();
    }

    private void ExecScrumMasterPlan()
    {
        
    }

    private void ExecDailyScrumPlan()
    {
        
    }

    private void InitPlayerAvatar()
    {
        player = Instantiate(Resources.Load<Player>("Prefabs/PlayerAvatar"));
        player.name = "PlayerAvatar";
        player.MoveInstantly(WaypointProvider.Spawn);
    }

    public void InitNewDay()
    {
        Day += 1;
        var id = DialogueIdProvider.GetDialogueIdByDay(Day);
        var advisorScreen = UiManager.Instance.AdvisorScreen;
        advisorScreen.DialogueBox.ShowDialogueById(id);
        
        advisorScreen.Show();
        GameState = GameState.ADVISE;
    }
    
    public void StartDay()
    {
        GameState = GameState.PLAYING;
        Instance.SongHandler?.PlaySongById(1);
        
        player.CanGiveCommand = true;

        InitCalls();
        
        Clock.SetTime(9,0,0);
        Clock.ResetAlarms();
        Clock.Running = true;
        GameSpeedController.Play();
    }
    
    public void InitDailyScrumPlan()
    {
        Company.AddEffectToCompanyScore(
            "Agilität", 
            "Neues Meeting: Daily Scrum", 
            10);
        
        Clock.SetAlarm(new TimeStamp(10,30,0), CallForDailyScrum, true);
        
        Clock.OnSecondTick += ExecDailyScrumPlan;
    }

    private void CallForDailyScrum()
    {
        NotificationController
            .DisplayNotification(
                "In 15 Minuten startet das Daily-Scrum-Meeting! " +
                "Wenn nicht jeder hingehen will, solltest du nachhelfen!"
                , NotificationType.Advisor);

        GameSpeedController.Play();
        
        MeetingRoomInteractible.CallForMeeting("Daily Scrum");
        Clock.SetAlarm(new TimeStamp(11,0,0), MeetingRoomInteractible.StartMeeting);
        Clock.SetAlarm(new TimeStamp(11,15,0), MeetingRoomInteractible.StopMeeting);
    }

    public void InitScrumMasterPlan()
    {
        Company.AddEffectToCompanyScore(
                "Agilität", 
                "Neue Rolle: Scrum Master", 
                10);

        ScrumMasterActive = true;
        Clock.OnSecondTick += ExecScrumMasterPlan;
    }
    
    public void InitTaskBoardPlan()
    {
        Company.AddEffectToCompanyScore(
            "Agilität", 
            "Neues Artefakt: Taskboard", 
            10);

        var taskboard = InteractibleManager.TaskboardInteractible;
        taskboard.gameObject.SetActive(true);
        taskboard.Stuff.SetActive(false);
        taskboard.LightContainer.GetComponentsInChildren<Light2D>().ToList().ForEach(light => { light.enabled = true; });
        
        InteractibleManager.AddToNpcInteractibles(taskboard);
    }
    
    public void AddToTeamspirit(string description, int value, Vector2 pos)
    {
        var score = UiManager.Instance.TeamspiritScore;
        void GainPointsCallback()
        {
            Company.AddEffectToCompanyScore("Teamgeist", description, value);
            if (value > 0)
            {
                SoundEffectManager.Instance.PlayRandomPop();
            }
            else
            {
                SoundEffectManager.Instance.PlayDud();
            }
        }
        
        TriggerTsEffect(Camera.main.WorldToScreenPoint(pos), score.transform.position, value, GainPointsCallback);
    }
    
    public void AddToAgility(string description, int value, Vector2 pos)
    {
        var score = UiManager.Instance.AgilityScore;
        void GainPointsCallback()
        {
            Company.AddEffectToCompanyScore("Agilität", description, value);
            if (value > 0)
            {
                SoundEffectManager.Instance.PlayRandomPop();
            }
            else
            {
                SoundEffectManager.Instance.PlayDud();
            }
        }
        
        TriggerAgiEffect(Camera.main.WorldToScreenPoint(pos), score.transform.position, value, GainPointsCallback);
    }
    
    public void AddToProgress(string description, int value, Vector2 pos)
    {
        var score = UiManager.Instance.ProgressScore;
        void GainPointsCallback()
        {
            Company.AddEffectToCompanyScore("Fortschritt", description, value);
            if (value > 0)
            {
                SoundEffectManager.Instance.PlayRandomPop();
            }
            else
            {
                SoundEffectManager.Instance.PlayDud();
            }
            
        }
        
        TriggerProgressEffect(Camera.main.WorldToScreenPoint(pos), score.transform.position, GainPointsCallback);
    }

    private void TriggerAgiEffect(Vector2 pos, Vector2 target, int value, Action callback)
    {
        if (value > 0)
        {
            EffectController.PlayAgiPlusEffectAt(pos, target, callback);    
        }
        else
        {
            EffectController.PlayAgiMinusEffectAt(pos, target, callback);   
        }
    }
    
    private void TriggerTsEffect(Vector2 pos, Vector2 target, int value, Action callback)
    {
        if (value > 0)
        {
            EffectController.PlayTsPlusEffectAt(pos, target, callback);    
        }
        else
        {
            EffectController.PlayTsMinusEffectAt(pos, target, callback);   
        }
    }
    
    private void TriggerProgressEffect(Vector2 pos, Vector2 target, Action callback)
    {
        EffectController.PlayProgressEffectAt(pos, target, callback);
    }
    
    public void FinishGame()
    {
        GameSpeedController.Pause();
        UiManager.Instance.FinishGameScreen.Show();
    }

    private void InitCalls()
    {
        switch (Day)
        {
            case 1:
                void Call1() => InteractibleManager.Phone.Ring(1);
                void Call2() => InteractibleManager.Phone.Ring(2);
                Clock.SetAlarm(new TimeStamp(11), Call1);
                Clock.SetAlarm(new TimeStamp(15), Call2);
                break;
            case 2:
                void Call3() => InteractibleManager.Phone.Ring(3);
                Clock.SetAlarm(new TimeStamp(13,30), Call3);
                break;
            case 3:
                void Call4() => InteractibleManager.Phone.Ring(4);
                Clock.SetAlarm(new TimeStamp(14,30), Call4);
                break;
            default:
                throw new NotImplementedException();
                break;
        }
            
    }
    
    #region Debug

    private int a = -1;
    public void DebugNotification()
    {
        a += 1;
        if (a == 0)
        {
            NotificationController.DisplayNotification("1 advisor test", NotificationType.Advisor);
        } else if (a == 1)
        {
            NotificationController.DisplayNotification("2 warning test", NotificationType.Warning);
        }
        else
        {
            NotificationController.DisplayNotification("3 default test", NotificationType.Default);
            a = -1;
        }
        
    }
    #endregion

}
