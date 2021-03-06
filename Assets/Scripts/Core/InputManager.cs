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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandlePauseToggle();
            }

            //only mouse inputs if unpaused
            if (GameManager.Instance.GameSpeedController.IsPaused) return;
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

            if (Input.mouseScrollDelta.y > 0.01f || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Camera.main.GetComponent<CameraHandler>().ZoomIn();
            }

            if (Input.mouseScrollDelta.y < -0.01f || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Camera.main.GetComponent<CameraHandler>().ZoomOut();
            }
        }
        private void HandlePauseToggle()
        {
            var gm = GameManager.Instance;
            if (gm.GameState != GameState.PLAYING) return;
            
            if (gm.GameSpeedController.IsPaused)
            {
                gm.GameSpeedController.UnPause();
            }
            else
            {
                gm.GameSpeedController.Pause();
            }
        }

        private void HandleColliderClick(RaycastHit2D[] hits)
        {
            CheckForInteractibleHits(hits);
            CheckForFloorHits(hits);
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

            var interactible =  hitInteractibles
                .Select(h => h.collider.GetComponent<Interactible>())
                .OrderByDescending(inter => inter.InteractionLayer)
                .First();

            GameManager.Instance.player.GiveInteractionOrder(interactible);
        }
    }
}