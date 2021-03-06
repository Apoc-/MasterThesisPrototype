﻿using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class WaypointProvider : MonoBehaviour
    {
        public Waypoint Spawn;
        public Waypoint[] ElevatorDoors;
        public Waypoint WaypointPrefab;

        public Waypoint CreateWaypointAtPosition(Vector2 position)
        {
            var wp = Instantiate(WaypointPrefab);
            wp.transform.position = position;
            return wp;
        }

        public void ClearWaypointActionsForEntity(Entity entity)
        {
            foreach (var wp in gameObject.GetComponentsInChildren<Waypoint>())
            {
                wp.UnregisterOnEnterActionsForEntity(entity);
            }
        }
    }
}