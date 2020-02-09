using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace Core
{
    public abstract class Entity : MonoBehaviour
    {
        public float walkingSpeed = 50;
        
        protected bool _hasMeeting;
        protected bool _reachedWalkTarget = true;
        protected bool _finishedInteraction = true;
        protected bool _startedInteraction = false;
        
        private Interactible _currentInteractTarget;
        private Action _finishedInteractionCallback;
        private float _interactionTime = 0.0f;
        
        private Queue<Waypoint> _walkTargets = new Queue<Waypoint>();
        private Waypoint _currentWalkTarget;
        private Vector2 _startWalkPosition;
        private float _startWalkTime = 0;
        private int _targetFloor = 0;
        private float _walkingDistance = 2.5f;
        private int _currentFloorId => GetCurrentFloorId();
        private Action _reachedWalkTargetCallback;

        private List<Waypoint> _helperWaypoints = new List<Waypoint>();
        private SpriteRenderer _spriteRenderer;
        protected SpriteRenderer SpriteRenderer
            => _spriteRenderer ? _spriteRenderer : _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        private void Start()
        {
            GameManager.Instance.Company.RegisterTeamMember(this);
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;

            if (!_finishedInteraction && ReachedInteractionTarget())
            {
                if (!_startedInteraction)
                {
                    _currentInteractTarget.StartInteraction(this);
                    _startedInteraction = true;
                }
                
                DoInteraction();
            }

            if (!_reachedWalkTarget)
            {
                HandleWalking();
                _reachedWalkTarget = ReachedWalkTarget();
            }
            
            if (_reachedWalkTarget && _currentWalkTarget != null)
            {
                _reachedWalkTargetCallback?.Invoke();
                _reachedWalkTargetCallback = null;
                _currentWalkTarget = null;
                CleanUpHelperWaypoints();
            }
        }

        private void CleanUpHelperWaypoints()
        {
            if (_helperWaypoints.Count > 0)
            {
                _helperWaypoints.ForEach(wp =>
                {
                    Destroy(wp.gameObject);
                });
            }
            
            _helperWaypoints.Clear();
        }

        private void DoInteraction()
        {
            _interactionTime += Time.deltaTime;

            if (_interactionTime >= _currentInteractTarget.InteractionDuration)
            {
                Debug.Log("Finished Interaction with " + _currentInteractTarget.GetName());
                _finishedInteraction = true;
                _startedInteraction = false;
                _finishedInteractionCallback?.Invoke();
                _currentInteractTarget.FinishInteraction(this);
            }
        }

        private bool ReachedInteractionTarget()
        {
            if (_currentInteractTarget == null) return false;

            var collider = GetComponent<Collider2D>();
            var targetCollider = _currentInteractTarget.gameObject.GetComponent<Collider2D>();
            return collider.bounds.Intersects(targetCollider.bounds);
        }

        private bool ReachedWalkTarget()
        {
            if (_currentWalkTarget == null) return true;

            var target = _currentWalkTarget.transform.position;
            var dist = Vector3.Distance(target, transform.position);
            return dist < _walkingDistance;
        }

        private bool HasTargetsLeft()
        {
            return _walkTargets.Count > 0;
        }

        private void HandleWalking()
        {
            if (_currentWalkTarget != null)
            {
                var covered = (Time.time - _startWalkTime) * walkingSpeed;
                var progress = covered / Vector2.Distance(_currentWalkTarget.transform.position, _startWalkPosition);
                SetLookingDirection();
                transform.position = Vector2.Lerp(_startWalkPosition, _currentWalkTarget.transform.position, progress);

                if (ReachedWalkTarget())
                {
                    _currentWalkTarget.TriggerOnEnterWaypoint(this);
                }
                
                if (ReachedWalkTarget() && HasTargetsLeft())
                {
                    _currentWalkTarget = _walkTargets.Dequeue();
                    _startWalkTime = Time.time;
                    _startWalkPosition = transform.position;
                }
            }
        }

        public void GiveInteractionOrder(Interactible interactible, Action finishedCallback = null)
        {
            GameManager.Instance.WaypointProvider.ClearWaypointActionsForEntity(this);
            _finishedInteraction = false;
            _finishedInteractionCallback = finishedCallback;
            _currentInteractTarget = interactible;
            _interactionTime = 0;
        }

        public void CancelCurrentInteractionOrder()
        {
            GameManager.Instance.WaypointProvider.ClearWaypointActionsForEntity(this);
            _finishedInteraction = true;
            _startedInteraction = false;
            _finishedInteractionCallback = null;
            _currentInteractTarget = null;
            _interactionTime = 0;
        }

        public void CancelAllOrders()
        {
            CancelCurrentInteractionOrder();
            CancelCurrentWalkOrder();
        }

        public void GiveWalkOrder(Vector2 target, int targetFloorId, Action reachedWalkTargetCallback = null)
        {
            GameManager.Instance.WaypointProvider.ClearWaypointActionsForEntity(this);
            _reachedWalkTarget = false;
            _reachedWalkTargetCallback = reachedWalkTargetCallback;

            var targetWp = GameManager.Instance.WaypointProvider.CreateWaypointAtPosition(target);
            _helperWaypoints.Add(targetWp);
            
            _walkTargets.Clear();
            _startWalkPosition = transform.position;
            _targetFloor = targetFloorId;
            _startWalkTime = Time.time;

            if (_targetFloor != _currentFloorId)
            {
                HandleFloorChange(targetWp, targetFloorId);
                _currentWalkTarget = _walkTargets.Dequeue();
            }
            else
            {
                _currentWalkTarget = targetWp;
            }
        }

        public void CancelCurrentWalkOrder()
        {
            GameManager.Instance.WaypointProvider.ClearWaypointActionsForEntity(this);
            _reachedWalkTarget = true;
            _reachedWalkTargetCallback = null;
            _walkTargets.Clear();
        }
        
        private void HandleFloorChange(Waypoint targetWp, int targetFloorId)
        {
            var elevatorDoorWps = GameManager.Instance.WaypointProvider.ElevatorDoors;
            var currentFloorWp = elevatorDoorWps[_currentFloorId];
            AddEnterElevatorEvent(currentFloorWp);

            var targetFloorWp = elevatorDoorWps[targetFloorId];
            AddLeaveElevatorEvent(targetFloorWp);

            _walkTargets.Enqueue(currentFloorWp);
            _walkTargets.Enqueue(targetFloorWp);
            _walkTargets.Enqueue(targetWp);
        }

        protected virtual void AddEnterElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                Hide();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
            };
            
            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        protected virtual void AddLeaveElevatorEvent(Waypoint waypoint)
        {
            UnityAction action = null;
            action = () =>
            {
                Show();
                waypoint.UnregisterOnEnterActionForEntity(this, action);
            };

            waypoint.RegisterOnEnterActionForEntity(this, action);
        }

        private void SetLookingDirection()
        {
            var x = transform.position.x;
            var targetX = _currentWalkTarget.transform.position.x;

            var scale = transform.localScale;
            scale.x = Mathf.Sign(x - targetX);
            transform.localScale = scale;
        }

        public void MoveInstantly(Waypoint targetWp)
        {
            var targetPos = targetWp.transform.position;
            transform.position = targetPos;
            _currentWalkTarget = targetWp;
            _startWalkPosition = targetPos;
        }


        private int GetCurrentFloorId()
        {
            var col = GetComponent<Collider2D>();
            List<Collider2D> overlap = new List<Collider2D>();
            col.OverlapCollider(new ContactFilter2D(), overlap);

            var floor = overlap
                .FindAll(c => c.GetComponent<Floor>() != null)
                .Select(c => c.GetComponent<Floor>())
                .First();
            
            return floor.floorId;
        }

        public void Show()
        {
            var col = SpriteRenderer.color;
            col.a = 1;
            SpriteRenderer.color = col;
            
            SpriteRenderer.gameObject.GetComponent<ShadowCaster2D>().castsShadows = true;
        }

        public void Hide()
        {
            var col = SpriteRenderer.color;
            col.a = 0;
            SpriteRenderer.color = col;

            SpriteRenderer.gameObject.GetComponent<ShadowCaster2D>().castsShadows = false;
        }

        public virtual void ReturnFromMeeting()
        {
            _hasMeeting = false;
            Show();
        }

        public abstract void CallToMeeting();
    }
}