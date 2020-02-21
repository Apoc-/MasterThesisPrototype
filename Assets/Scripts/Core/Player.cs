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

        private bool _isWorking = false;
        public Jun_TweenRuntime BopAnimation;
        private float _progressTimer = 0;
        
        public void StartWork()
        {
            //hacky stop walk
            //BopAnimation.enabled = false;
            _isWorking = true;
        }
        
        public void StopWork()
        {
            //hacky start walk
            //BopAnimation.enabled = true;
            _isWorking = false;
        }
        
        private void Update()
        {
            if (_isWorking)
            {
                MakeProgress();
            }
            
            if (ReachedInteractionTarget())
            {
                if (CurrentInteractTarget is Fixable fixable && fixable.IsBroken || CurrentInteractTarget is Chair)
                {
                    EnableInteractionIcon();
                }
            }

            if (_finishedInteraction || !ReachedInteractionTarget())
            {
                DisableInteractionIcon();
                StopWork();
            }
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