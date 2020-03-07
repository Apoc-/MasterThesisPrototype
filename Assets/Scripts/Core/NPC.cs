using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Code;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Core
{
    public class NPC : Entity, IHasToolTip
    {
        public string Name;
        [SerializeField] private float _meetingAttendChance = 0.5f;
        [SerializeField] private float _minimumOfficeTime = 20f;
        [SerializeField] private float _baseBreakChance = 0.2f;
        [SerializeField] private float _currentBreakChance = 0.2f;
        [SerializeField] private float _needHelpChance = 0.1f;
        

        private float _helpTimer = 0f;
        private float _officeTimer = 0f;
        public Interactible Office;

        private bool _isInOffice = false;
        private float _progressTimer = 0f;
        public bool NeedsHelp { get; set; } = false;
        private HelpWarningSign _helpWarningSign;

        public GameObject AnimationGameObject;
        
        private bool _drankMorningCoffee = false;

        public void Update()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;

            if (_isInOffice && !NeedsHelp)
            {
                CheckForHelpTick();
                DoWork();
            }

            if (CanMakeDecision() && FinishedOfficeTime())
            {
                DecideAction();
            }
            
            if (ReachedInteractionTarget() || _isInOffice)
            {
                EnableInteractionIcon();
            }

            if (!(ReachedInteractionTarget() || _isInOffice) || NeedsHelp)
            {
                DisableInteractionIcon();
            }
        }

        private void CheckForHelpTick()
        {
            _helpTimer -= Time.deltaTime;
            if (_helpTimer >= 0)
            {
                return;
            }

            _helpTimer = 1;
            
            if (Random.Range(0f, 1f) < _needHelpChance)
            {
                NeedsHelp = true;
                DisplayHelpWarning();
            }
        }

        private bool FinishedOfficeTime()
        {
            _officeTimer += Time.deltaTime;
            var timerElapsed = _officeTimer > _minimumOfficeTime;

            return timerElapsed;
        }

        private void DisplayHelpWarning()
        {
            var pos = GetSignPosition();
            var sign = Instantiate(
                Resources.Load<GameObject>("Prefabs/HelpWarningSign"),
                AnimationGameObject.transform);

            sign.transform.position = pos;
            _helpWarningSign = sign.GetComponent<HelpWarningSign>();
            _helpWarningSign.AttachedNPC = this;
            GameManager.Instance.TasklistScreenBehaviour.AddImpediment(_helpWarningSign);
        }

        private Vector3 GetSignPosition()
        {
            return SpriteRenderer.bounds.center;
        }

        private void OnEnable()
        {
            _officeTimer = _minimumOfficeTime;
        }

        private bool CanMakeDecision()
        {
            return !HasMeeting && _finishedInteraction && _reachedWalkTarget && !NeedsHelp;
        }
        
        private void DecideAction()
        {
            if (!_drankMorningCoffee)
            {
                if (_isInOffice) LeaveOffice();
                var coffeeMachine = GameManager.Instance
                    .InteractibleManager
                    .NpcInteractibles.First(interactible => interactible is CoffeeMachine);

                InteractWith(coffeeMachine, () =>
                {
                    _drankMorningCoffee = true;
                    GoBackToOffice();
                });
            }
            else
            {
                DoRandomInteraction();
            }
        }

        private void DoRandomInteraction()
        {
            var interactible = GetRandomNpcInteractible();
            if (_isInOffice) LeaveOffice();
            InteractWith(interactible, () =>
            {
                HandleRandomBreak(interactible);
                GoBackToOffice();
            });
        }

        private void HandleRandomBreak(Interactible interactible)
        {
            if (!GameManager.Instance.ScrumMasterActive) return;

            var fixable = interactible as Fixable;
            if (fixable == null) return;

            var rnd = Random.Range(0, 1f);
            if (rnd <= _currentBreakChance)
            {
                fixable.Break();
                _currentBreakChance = _baseBreakChance;
            }
            else
            {
                _currentBreakChance += _baseBreakChance;
            }
        }

        private void GoBackToOffice()
        {
            var floor = Office.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(Office.transform.position.x, floorCollider.bounds.min.y);

            GiveWalkOrder(walkTarget, floor.floorId, EnterOffice);
        }

        private void EnterOffice()
        {
            SoundEffectManager.Instance.PlayDoorSound();
            _isInOffice = true;
            _officeTimer = 0;
            Hide();
        }

        private void LeaveOffice()
        {
            SoundEffectManager.Instance.PlayDoorSound();
            _isInOffice = false;
            Show();
        }

        private void DoWork()
        {
            _progressTimer -= Time.deltaTime;
            if (_progressTimer <= 0)
            {
                GameManager.Instance.AddToProgress("Fortschritt", 1, OverheadPosition);
                _progressTimer = GameManager.Instance.Company.GetProgressTimer();
            }
        }

        private Interactible GetRandomNpcInteractible()
        {
            var interactibles = GameManager.Instance
                .InteractibleManager
                .NpcInteractibles
                .ToList();

            var rand = Random.Range(0, interactibles.Count);

            return interactibles.ToList()[rand];
        }

        private void InteractWith(Interactible interactible, Action finishedCallback = null)
        {
            var floor = interactible.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(interactible.GetNPCWalkTarget().x, floorCollider.bounds.min.y);

            GiveInteractionOrder(interactible, finishedCallback);
            GiveWalkOrder(walkTarget, floor.floorId);
        }

        public override void CallToMeeting(MeetingRoomInteractible meetingRoomInteractible)
        {
            if (Random.Range(0f, 1f) < _meetingAttendChance)
            {
                GiveGoToMeetingCommand(meetingRoomInteractible);
            }
            else
            {
                DontAttendMeeting(meetingRoomInteractible);
            }
        }

        private void DontAttendMeeting(MeetingRoomInteractible meetingRoomInteractible)
        {
            HasMeeting = true;
            DisplayMeetingWarning(meetingRoomInteractible);
        }

        private void DisplayMeetingWarning(MeetingRoomInteractible meetingRoomInteractible)
        {
            var pos = GetSignPosition();
            var sign = Instantiate(
                Resources.Load<GameObject>("Prefabs/MeetingWarningSign"),
                AnimationGameObject.transform);

            sign.transform.position = pos;
            var signInteractible = sign.GetComponent<MeetingWarningSign>();
            signInteractible.MeetingRoomInteractible = meetingRoomInteractible;
            signInteractible.AttachedNPC = this;
            GameManager.Instance.TasklistScreenBehaviour.AddImpediment(signInteractible);
        }

        public void GiveGoToMeetingCommand(MeetingRoomInteractible meetingRoomInteractible)
        {
            if (_isInOffice) LeaveOffice();

            CancelAllOrders();
            HasMeeting = true;
            _isRunning = true;

            GiveWalkOrder(
                meetingRoomInteractible.GetNPCWalkTarget(),
                meetingRoomInteractible.GetFloor().floorId,
                () => { meetingRoomInteractible.EnterMeeting(this); });
        }

        public override void ReturnFromMeeting()
        {
            base.ReturnFromMeeting();

            var sign = GetComponentInChildren<MeetingWarningSign>();

            if (sign != null)
            {
                GameManager.Instance.TasklistScreenBehaviour.RemoveImpediment(sign);
                Destroy(sign.gameObject);
            }

            _isRunning = false;
            GoBackToOffice();
        }

        public string GetTooltip() => (_isInOffice || _isInElevator || HasMeeting) ? "" : Name;

        public void MakeThirsty()
        {
            CancelAllOrders();
            _drankMorningCoffee = false;
        }

        public void HelpNpc()
        {
            _helpWarningSign.FinishInteraction(this);
        }
    }
}