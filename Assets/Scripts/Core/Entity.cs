using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

namespace Core
{
    public class Entity : MonoBehaviour
    {
        public float walkingSpeed = 50;
        private Queue<Vector2> _walkTargets = new Queue<Vector2>();
        private Vector2 _currentWalkTarget;
        
        private Vector2 _startWalkPosition;
        private float _startWalkTime = 0;
        private int _targetFloor = 0;
        private float _actingDistance = 50f;
        private int _currentFloorId => GetCurrentFloorId();
        
        private bool reachedWalkTarget = true;
        private Action _reachedWalkTargetCallback;
        
        protected Interactible _currentInteractTarget;
        private float _interactionTime = 0.0f;
        private bool _finishedInteraction = true;
        private Action _finishedInteractionCallback;
        

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;

            if (!_finishedInteraction && ReachedInteractionTarget())
            {
                DoInteraction();
            }

            if (!reachedWalkTarget)
            {
                HandleWalking();
                reachedWalkTarget = ReachedWalkTarget();

                if (reachedWalkTarget)
                {
                    _reachedWalkTargetCallback?.Invoke();
                }
            }
        }

        private void DoInteraction()
        {
            _interactionTime += Time.deltaTime;

            if (_interactionTime >= _currentInteractTarget.InteractionDuration)
            {
                Debug.Log("Finished Interaction with " + _currentInteractTarget.GetName());
                _finishedInteraction = true;
                _finishedInteractionCallback?.Invoke();
                _currentInteractTarget.FinishInteraction();
            }
        }

        private bool ReachedInteractionTarget()
        {
            if (_currentInteractTarget == null) return false;
            
            var dist = Vector2.Distance(_currentInteractTarget.transform.position, transform.position);
            return dist < _actingDistance;
        }

        private bool ReachedWalkTarget()
        {
            return Vector2.Distance(_currentWalkTarget, transform.position) <= 0.01f;
        }

        private bool HasTargetsLeft()
        {
            return _walkTargets.Count > 0;
        }

        private void HandleWalking()
        {
            var covered = (Time.time - _startWalkTime) * walkingSpeed;
            var progress = covered / Vector2.Distance(_currentWalkTarget, _startWalkPosition);
            SetLookingDirection();
            transform.position = Vector2.Lerp(_startWalkPosition, _currentWalkTarget, progress);
            
            if (ReachedWalkTarget() && HasTargetsLeft())
            {
                _currentWalkTarget = _walkTargets.Dequeue();
                _startWalkTime = Time.time;
                _startWalkPosition = transform.position;
            }
        }

        public void GiveInteractionOrder(Interactible interactible, Action finishedCallback = null)
        {
            _finishedInteraction = false;
            _finishedInteractionCallback = finishedCallback;
            _currentInteractTarget = interactible;
            _interactionTime = 0;
        }
        
        public void GiveWalkOrder(Vector2 target, int targetFloorId, Action reachedWalkTargetCallback = null)
        {
            reachedWalkTarget = false;
            _reachedWalkTargetCallback = reachedWalkTargetCallback; 
            var dist = Vector2.Distance(transform.position, target);
            if (dist < _actingDistance / 10f) return;
            
            _walkTargets.Clear();
            _startWalkPosition = transform.position;
            _targetFloor = targetFloorId;
            _startWalkTime = Time.time;

            if (_targetFloor != _currentFloorId)
            {
                HandleFloorChange(target, targetFloorId);
                _currentWalkTarget = _walkTargets.Dequeue();
            }
            else
            {
                _currentWalkTarget = target;
            }
        }

        private void HandleFloorChange(Vector2 target, int targetFloorId)
        {
            var elevatorDoorWps = GameManager.Instance.WaypointProvider.ElevatorDoors;
            var currentFloorWp = elevatorDoorWps[_currentFloorId].transform.position;
            var targetFloorWp = elevatorDoorWps[targetFloorId].transform.position;
            
            _walkTargets.Enqueue(currentFloorWp);
            _walkTargets.Enqueue(targetFloorWp);
            _walkTargets.Enqueue(target);
        }

        private void SetLookingDirection()
        {
            var x = transform.position.x;
            var targetX = _currentWalkTarget.x;

            var scale = transform.localScale;
            scale.x = Mathf.Sign(x-targetX);
            transform.localScale = scale;
        }

        public void MoveInstantly(Vector2 targetPosition, int floorId)
        {
            transform.position = targetPosition;
            _currentWalkTarget = targetPosition;
            _startWalkPosition = targetPosition;
        }
        
        
        private int GetCurrentFloorId()
        {
            var col = GetComponent<Collider2D>();
            List<Collider2D> overlap = new List<Collider2D>();
            col.OverlapCollider(new ContactFilter2D(), overlap);
            
            var floor = overlap.FindAll(c => c.GetComponent<Floor>() != null).Select(c => c.GetComponent<Floor>()).First();
            return floor.floorId;
        }
    }
}