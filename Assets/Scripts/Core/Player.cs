using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Player : Entity
    {
        public bool CanGiveMoveCommand = false;

        protected override void AddEnterElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                Hide();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
                CanGiveMoveCommand = false;
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
                CanGiveMoveCommand = true;
            };

            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        public override void CallToMeeting()
        {
            
        }

        public override void ReturnFromMeeting()
        {
            base.ReturnFromMeeting();

            CanGiveMoveCommand = true;
        }
    }
}