using System;
using System.Collections.Generic;
using System.Linq;
using Tasklist;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Player : Entity
    {
        public bool CanGiveCommand = false;
        public void DisableCommands() => CanGiveCommand = false;
        public void  EnableCommands() => CanGiveCommand = true;
        
        private bool _isWorking = false;
        public void StartWork() => _isWorking = true;
        public void StopWork() => _isWorking = false;
        
        public Jun_TweenRuntime BopAnimation;
        private float _progressTimer = 0;

        private void Update()
        {
            if (_isWorking)
            {
                MakeProgress();
            }
            
            if (ReachedInteractionTarget())
            {
                if (CurrentInteractTarget is Fixable fixable && fixable.IsBroken 
                    || CurrentInteractTarget is Chair)
                {
                    EnableInteractionIcon();
                }
            }

            if (_finishedInteraction || !ReachedInteractionTarget() || GameManager.Instance.GameState != GameState.PLAYING)
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
                GameManager.Instance.TasklistScreenBehaviour.ReportTaskProgress(BonusTaskType.MakeProgress);
            }
        }

        protected override void AddEnterElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                EnableOutline();
                Hide();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
                DisableCommands();
            };
            
            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        protected override void AddLeaveElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                DisableOutline();
                Show();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
                EnableCommands();
            };

            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        

        public override void CallToMeeting(MeetingRoomInteractible interactible)
        {
            
        }

        public void EnterMeeting()
        {
            _hasMeeting = true;
            DisableCommands();
        }
        
        public override void ReturnFromMeeting()
        {
            base.ReturnFromMeeting();
            EnableCommands();
        }
    }
}