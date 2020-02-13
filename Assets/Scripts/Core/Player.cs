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
        public GameObject WorkingIcon;

        public void Update()
        {
            if (_startedInteraction)
            {
                EnableWorkingIcon();
            }

            if (_finishedInteraction || !ReachedInteractionTarget())
            {
                DisableWorkingIcon();
            }
        }

        private void DisableWorkingIcon()
        {
            if(WorkingIcon.activeSelf) WorkingIcon.SetActive(false);
        }

        private void EnableWorkingIcon()
        {
            if(!WorkingIcon.activeSelf) WorkingIcon.SetActive(true);
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

        public override void CallToMeeting()
        {
            
        }

        public override void ReturnFromMeeting()
        {
            base.ReturnFromMeeting();

            CanGiveCommand = true;
        }
    }
}