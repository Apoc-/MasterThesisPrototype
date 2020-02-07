using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Waypoint : MonoBehaviour
    {
        private readonly Dictionary<Entity, UnityEvent> _onEnterWaypointTriggers = new Dictionary<Entity, UnityEvent>();
        public void TriggerOnEnterWaypoint(Entity entity)
        {
            if (!_onEnterWaypointTriggers.ContainsKey(entity)) return;
            _onEnterWaypointTriggers[entity]?.Invoke();
        }

        public void UnregisterOnEnterActionsForEntity(Entity entity)
        {
            if (!_onEnterWaypointTriggers.ContainsKey(entity)) return;
            _onEnterWaypointTriggers[entity].RemoveAllListeners();
        }

        public void UnregisterOnEnterActionForEntity(Entity entity, UnityAction action)
        {
            if (!_onEnterWaypointTriggers.ContainsKey(entity)) return;
            _onEnterWaypointTriggers[entity].RemoveListener(action);
        }
        
        public void RegisterOnEnterActionForEntity(Entity entity, UnityAction action)
        {
            if (_onEnterWaypointTriggers.ContainsKey(entity))
            {
                _onEnterWaypointTriggers[entity].AddListener(action);
            }
            else
            {
                _onEnterWaypointTriggers.Add(entity, new UnityEvent());
                _onEnterWaypointTriggers[entity].AddListener(action);
            }
        }
    }
}