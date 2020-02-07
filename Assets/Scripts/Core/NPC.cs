using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Core
{
    public class NPC : Entity
    {
        public string Name;
        [SerializeField] private float _activity = 1f; //seconds per decision
        [SerializeField] private float _randomInteractionChance = 0.8f;
        [SerializeField] private float _meetingAttendChance = 0.5f;

        private float _decisionTimer = 0f;
        public Interactible Office;
        private bool _isInOffice = true;
        public void Update()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;
            
            if (CanMakeDecision())
            {
                DecideAction();
            }
        }

        private bool CanMakeDecision()
        {
            _decisionTimer += Time.deltaTime;
            var decisionTimerElapsed = _decisionTimer > _activity;
            return decisionTimerElapsed && !_hasMeeting && _finishedInteraction && _reachedWalkTarget;
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
            _isInOffice = false;
            var interactible = GetRandomNpcInteractible();
            InteractWith(interactible, GoBackToOffice);
        }

        private void GoBackToOffice()
        {
            Debug.Log("Going back to my office");
            var floor = Office.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(Office.transform.position.x, floorCollider.bounds.min.y);
            
            GiveWalkOrder(walkTarget, floor.floorId, () =>
            {
                _isInOffice = true;
                _decisionTimer = 0;
            });
        }

        private Interactible GetRandomNpcInteractible()
        {
            var interactibles = GameManager.Instance.InteractibleManager.NpcInteractibles;
            var rand = Random.Range(0, interactibles.Count);

            return interactibles.ToList()[rand];
        }

        private void InteractWith(Interactible interactible, Action finishedCallback)
        {
            var floor = interactible.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(interactible.transform.position.x, floorCollider.bounds.min.y);
            
            GiveInteractionOrder(interactible, finishedCallback);
            GiveWalkOrder(walkTarget, floor.floorId);
        }

        public override void CallToMeeting()
        {
            if (Random.Range(0f, 1f) < _meetingAttendChance) GoToMeeting();
        }

        private void GoToMeeting()
        {
            CancelAllOrders();

            _hasMeeting = true;
            var meetingRoomBehaviour = GameManager.Instance.MeetingRoomBehaviour;
            Vector2 pos = new Vector2(meetingRoomBehaviour.gameObject.transform.position.x, 0);

            void EnterMeetingRoom()
            {
                meetingRoomBehaviour.EnterMeeting(this);
            }

            GiveWalkOrder(pos, 2, EnterMeetingRoom);
        }
        
        public new void CancelAllOrders()
        {
            CancelCurrentInteractionOrder();
            CancelCurrentWalkOrder();
        }
    }
}