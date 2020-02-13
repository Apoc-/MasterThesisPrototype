using System;
using System.Collections;
using System.Collections.Generic;
using Code;
using Core;
using Tech;
using UI;
using Unity.Collections;
using UnityEngine;
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

    private MeetingRoomBehaviour _meetingRoomBehaviour;
    public MeetingRoomBehaviour MeetingRoomBehaviour
        => _meetingRoomBehaviour
            ? _meetingRoomBehaviour
            : _meetingRoomBehaviour = FindObjectOfType<MeetingRoomBehaviour>();
    
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

    private Clock _clock;
    public Clock Clock => _clock ? _clock : _clock = FindObjectOfType<Clock>();
    
    [ReadOnly] public Player player;

    public GameState GameState = GameState.INIT;

    public InteractibleManager InteractibleManager;

    public int Day = 0;
    
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
        Clock.SetAlarm(new TimeStamp(17,00,0), FinishDay, true);
    }

    private void InitCompany()
    {
        Company = new Company(name);
    }
    
    private void FinishDay()
    {
        GameState = GameState.ADVISE;
        Clock.Running = false;
        UiManager.Instance.ShowScoreScreen();
    }

    private void ExecScrumMasterPlan()
    {
        if (Random.Range(0f, 1f) < _impedimentChance)
        {
            InteractibleManager.BreakRandomBreakable();
        }
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
        player.CanGiveMoveCommand = true;
        Clock.SetTime(9,0,0);
        Clock.ResetAlarms();
        Clock.Running = true;
    }
    
    public void InitDailyScrumPlan()
    {
        Company.AddEffectToCompanyScore(
            "Agilität", 
            "Neues Meeting: Daily Scrum", 
            10);
        
        Clock.SetAlarm(new TimeStamp(10,45,0), CallForDailyScrum, true);
        
        Clock.OnSecondTick += ExecDailyScrumPlan;
    }

    private void CallForDailyScrum()
    {
        NotificationController
            .DisplayNotification(
                "Das Daily-Scrum-Meeting startet für jeden (auch dich) in 15 Minuten. " +
                "Wenn mal wieder nicht jeder kommen will, solltest du nachhelfen!"
                , NotificationType.Advisor);
        
        MeetingRoomBehaviour.CallForMeeting("Daily Scrum");
        Clock.SetAlarm(new TimeStamp(11,0,0), MeetingRoomBehaviour.StartMeeting);
        Clock.SetAlarm(new TimeStamp(11,15,0), MeetingRoomBehaviour.StopMeeting);
    }

    public void InitScrumMasterPlan()
    {
        Company.AddEffectToCompanyScore(
                "Agilität", 
                "Neue Rolle: Scrum Master", 
                10);
        
        // add problems for master
        Clock.OnSecondTick += () =>
        {
            
        };

        Clock.OnSecondTick += ExecScrumMasterPlan;
    }
    
    public void AddToTeamspirit(string description, int value, Vector2 pos)
    {
        var score = UiManager.Instance.TeamspiritScore;
        void GainPointsCallback()
        {
            Company.AddEffectToCompanyScore("Teamgeist", description, value);
        }
        
        TriggerEffect(Camera.main.WorldToScreenPoint(pos), score.transform.position, value, GainPointsCallback);
    }
    
    public void AddToAgility(string description, int value, Vector2 pos)
    {
        var score = UiManager.Instance.AgilityScore;
        void GainPointsCallback()
        {
            Company.AddEffectToCompanyScore("Agilität", description, value);
        }
        
        TriggerEffect(Camera.main.WorldToScreenPoint(pos), score.transform.position, value, GainPointsCallback);
    }

    private void TriggerEffect(Vector2 pos, Vector2 target, int value, Action callback)
    {
        if (value > 0)
        {
            EffectController.PlayPlusEffectAt(pos, target, callback);    
        }
        else
        {
            EffectController.PlayMinusEffectAt(pos, target, callback);   
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
