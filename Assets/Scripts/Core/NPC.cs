using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class NPC : Entity
    {
        public string Name;
        [SerializeField] private float _activity = 1f; //seconds per decision
        [SerializeField] private float _randomInteractionChance = 0.8f;
        private float _decisionTimer = 0f;
        private bool _isBusy = false;
        public Interactible Office;
        public void Update()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;
            
            _decisionTimer += Time.deltaTime;
            
            if (!_isBusy && _decisionTimer > _activity)
            {
                DecideAction();
            }
        }

        private void DecideAction()
        {
            if (Random.Range(0f, 1f) < _randomInteractionChance)
            {
                DoRandomInteraction();
            }
        }

        private void DoRandomInteraction()
        {
            _isBusy = true;
            var interactible = GetRandomNpcInteractible();
            InteractWith(interactible, GoBackToOffice);
        }

        private void GoBackToOffice()
        {
            Debug.Log("Going back to my office");
            var floor = Office.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(Office.transform.position.x, floorCollider.bounds.min.y);
            
            GiveWalkOrder(walkTarget, floor.floorId, () =>
            {
                _isBusy = false;
                _decisionTimer = 0;
            });
        }

        private Interactible GetRandomNpcInteractible()
        {
            var interactibles = GameManager.Instance.InteractibleManager.NpcInteractibles;
            var rand = Random.Range(0, interactibles.Count);

            return interactibles.ToList()[rand];
        }

        private void InteractWith(Interactible interactible, Action finishedCallback)
        {
            var floor = interactible.GetFloor();
            var floorCollider = floor.GetComponent<Collider2D>();
            var walkTarget = new Vector2(interactible.transform.position.x, floorCollider.bounds.min.y);
            
            GiveInteractionOrder(interactible, finishedCallback);
            GiveWalkOrder(walkTarget, floor.floorId);
        }
    }
}