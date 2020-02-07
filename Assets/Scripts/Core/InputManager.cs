using System;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance.GameState != GameState.PLAYING) return;

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
            CheckForInteractibleHits(hits);
            CheckForFloorHits(hits);
        }

        private void CheckForFloorHits(RaycastHit2D[] hits)
        {
            var hitFloors = hits.ToList()
                .Where(h => h.collider.GetComponent<Floor>() != null)
                .ToList();

            if (hitFloors.Count == 0) return;

            var hit = hitFloors[0];
            var floor = hitFloors[0].collider.GetComponent<Floor>();
            var targetFloorId = floor.floorId;
            var target = new Vector2(hit.point.x, hit.collider.bounds.min.y);
            var player = GameManager.Instance.player;
            
            if(player.CanGiveMoveCommand) player.GiveWalkOrder(target, targetFloorId);
        }

        private void CheckForInteractibleHits(RaycastHit2D[] hits)
        {
            var hitInteractibles = hits.ToList()
                .Where(h => h.collider.GetComponent<Interactible>() != null)
                .ToList();

            if (hitInteractibles.Count == 0) return;

            var interactibles = hitInteractibles.Select(h => h.collider.GetComponent<Interactible>());
            foreach (var interactible in interactibles)
            {
                GameManager.Instance.player.GiveInteractionOrder(interactible);
            }
        }
    }
}