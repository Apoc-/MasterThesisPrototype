using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Core
{
    public class NPC : Entity, IHasToolTip
    {
        public string Name;
        [SerializeField] private float _activity = 1f; //seconds per decision
        [SerializeField] private float _randomInteractionChance = 0.8f;
        [SerializeField] private float _meetingAttendChance = 0.5f;
        [SerializeField] private float _minimumOfficeTime = 20f;
        [SerializeField] private float _breakChance = 0.2f;
        
        private float _decisionTimer = 0f;
        private float _officeTimer = 0f;
        public Interactible Office;

        private bool _isInOffice = false;
        private float _progressTimer = 0f;
        
        public void Update()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;

            if (ReachedInteractionTarget() || _isInOffice)
            {
                EnableInteractionIcon();
            }
            
            if (CanMakeDecision() && CanTakeAction())
            {
                DecideAction();
            }

            if (_isInOffice)
            {
                MakeProgress();
            }

            if (!(ReachedInteractionTarget() || _isInOffice))
            {
                DisableInteractionIcon();
            }
        }

        private void OnEnable()
        {
            _decisionTimer = _activity;
            _officeTimer = _minimumOfficeTime;
        }

        private bool CanMakeDecision()
        {
            _decisionTimer += Time.deltaTime;
            var decisionTimerElapsed = _decisionTimer > _activity;
            return decisionTimerElapsed && !_hasMeeting && _finishedInteraction && _reachedWalkTarget;
        }

        private bool CanTakeAction()
        {
            _officeTimer += Time.deltaTime;
            var timerElapsed = _officeTimer > _minimumOfficeTime;
            
            return timerElapsed;
        }

        private void DecideAction()
        {
            if (Random.Range(0f, 1f) < _randomInteractionChance)
            {
                DoRandomInteraction();
            }
        }

        private void DoRandomInteraction()
        {
            LeaveOffice();
            var interactible = GetRandomNpcInteractible();
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
            if (rnd <= _breakChance)
            {
                fixable.Break();
            }
        }
        
        private void GoBackToOffice()
        {
            var floor = Office.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(Office.transform.position.x, floorCollider.bounds.min.y);

            GiveWalkOrder(walkTarget, floor.floorId, () =>
            {
                EnterOffice();
                _decisionTimer = 0;
            });
        }

        private void EnterOffice()
        {
            _isInOffice = true;
            _officeTimer = 0;
            Hide();
        }

        private void LeaveOffice()
        {
            _isInOffice = false;
            Show();
        }

        private void MakeProgress()
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
            var walkTarget = new Vector2(interactible.transform.position.x, floorCollider.bounds.min.y);

            GiveInteractionOrder(interactible, finishedCallback);
            GiveWalkOrder(walkTarget, floor.floorId);
        }

        public override void CallToMeeting(MeetingRoomInteractible meetingRoomInteractible)
        {
            if (Random.Range(0f, 1f) < _meetingAttendChance)
            {
                GoToMeeting(meetingRoomInteractible);
            }
            else
            {
                DontAttendMeeting(meetingRoomInteractible);
            }
        }

        private void DontAttendMeeting(MeetingRoomInteractible meetingRoomInteractible)
        {
            _hasMeeting = true;
            DisplayMeetingWarning(meetingRoomInteractible);
        }

        private void DisplayMeetingWarning(MeetingRoomInteractible meetingRoomInteractible)
        {
            var pos = SpriteRenderer.bounds.center;
            var sign = Instantiate(
                Resources.Load<GameObject>("Prefabs/MeetingWarningSign"),
                transform);

            sign.transform.position = pos;
            var signInteractible = sign.GetComponent<MeetingWarningSign>();
            signInteractible.MeetingRoomInteractible = meetingRoomInteractible;
            signInteractible.AttachedNPC = this;
        }

        public void GoToMeeting(MeetingRoomInteractible meetingRoomInteractible)
        {
            if(_isInOffice) LeaveOffice();
            
            CancelAllOrders();
            _hasMeeting = true;
            _isRunning = true;

            InteractWith(meetingRoomInteractible);
        }

        public override void ReturnFromMeeting()
        {
            base.ReturnFromMeeting();

            var sign = GetComponentInChildren<MeetingWarningSign>();
            if (sign != null)
            {
                Destroy(sign.gameObject);
            }

            _isRunning = false;
        }

        public string GetTooltip() => (_isInOffice || _isInElevator || _hasMeeting) ? "" : Name;
    }
}