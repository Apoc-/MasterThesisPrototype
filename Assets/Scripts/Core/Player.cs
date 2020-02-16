using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Player : Entity
    {
        public bool CanGiveCommand = false;

        private void Update()
        {
            if (ReachedInteractionTarget())
            {
                if (CurrentInteractTarget is Fixable fixable && fixable.IsBroken)
                {
                    EnableInteractionIcon();
                }
            }

            if (_finishedInteraction || !ReachedInteractionTarget())
            {
                DisableInteractionIcon();
            }
        }

        protected override void AddEnterElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                Hide();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
                CanGiveCommand = false;
            };
            
            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        protected override void AddLeaveElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                Show();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
                CanGiveCommand = true;
            };

            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        public override void CallToMeeting(MeetingRoomInteractible interactible)
        {
            
        }

        public override void ReturnFromMeeting()
        {
            base.ReturnFromMeeting();

            CanGiveCommand = true;
        }
    }
}