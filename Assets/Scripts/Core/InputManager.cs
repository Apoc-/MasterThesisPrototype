﻿using System;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance.GameState != GameState.PLAYING) return;
                if (EventSystem.current.IsPointerOverGameObject()) return;
                
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

                if (hits.Length > 0)
                {
                    HandleColliderClick(hits);
                }
            }

            if (Input.mouseScrollDelta.y > 0.01f)
            {
                Camera.main.GetComponent<CameraHandler>().ZoomIn();
            }

            if (Input.mouseScrollDelta.y < -0.01f)
            {
                Camera.main.GetComponent<CameraHandler>().ZoomOut();
            }
        }

        private void HandleColliderClick(RaycastHit2D[] hits)
        {
            var handled = CheckForMeetingRoomHit(hits);
            if (!handled) CheckForInteractibleHits(hits);
            if (!handled) CheckForFloorHits(hits);
        }

        private bool CheckForMeetingRoomHit(RaycastHit2D[] hits)
        {
            var player = GameManager.Instance.player;
            if (!player.CanGiveCommand) return true;

            if (!GameManager.Instance.MeetingRoomBehaviour.HasMeeting) return false;

            var hasHitMeetingRoom = hits.Count(h => h.collider.GetComponent<MeetingRoomBehaviour>() != null) > 0;
            if (!hasHitMeetingRoom) return false;

            var meetingRoomHit = hits.ToList().First(h => h.collider.GetComponent<MeetingRoomBehaviour>() != null);
            var meetingRoom = meetingRoomHit.collider.GetComponent<MeetingRoomBehaviour>();

            var pos = new Vector2(meetingRoom.gameObject.transform.position.x, 0);
            player.GiveWalkOrder(pos, 2,
                () =>
                {
                    player.CanGiveCommand = false;
                    meetingRoom.EnterMeeting(player);
                });


            return true;
        }

        private void CheckForFloorHits(RaycastHit2D[] hits)
        {
            var player = GameManager.Instance.player;
            if (!player.CanGiveCommand) return;

            var hitFloors = hits.ToList()
                .Where(h => h.collider.GetComponent<Floor>() != null)
                .ToList();

            if (hitFloors.Count == 0) return;

            var hit = hitFloors[0];
            var floor = hitFloors[0].collider.GetComponent<Floor>();
            var targetFloorId = floor.floorId;
            var target = new Vector2(hit.point.x, hit.collider.bounds.min.y);


            player.GiveWalkOrder(target, targetFloorId);
        }

        private void CheckForInteractibleHits(RaycastHit2D[] hits)
        {
            var player = GameManager.Instance.player;
            if (!player.CanGiveCommand) return;

            var hitInteractibles = hits.ToList()
                .Where(h => h.collider.GetComponent<Interactible>() != null)
                .ToList();

            if (hitInteractibles.Count == 0) return;

            var interactible = hitInteractibles.Select(h => h.collider.GetComponent<Interactible>()).First();
            GameManager.Instance.player.GiveInteractionOrder(interactible);
        }
    }
}