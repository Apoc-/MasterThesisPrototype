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
                var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                var color = spriteRenderer.color;
                color.a = 0;
                spriteRenderer.color = color;
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
                var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                var color = spriteRenderer.color;
                color.a = 1;
                spriteRenderer.color = color;
                waypoint.UnregisterOnEnterActionForEntity(this, action);
                CanGiveMoveCommand = true;
            };

            waypoint.RegisterOnEnterActionForEntity(this, action);
        }
    }
}