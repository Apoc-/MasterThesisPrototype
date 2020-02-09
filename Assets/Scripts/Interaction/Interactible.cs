using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Interactible : MonoBehaviour, IHasToolTip
    {
        public float InteractionDuration = 3;
        private IHasToolTip _hasToolTipImplementation;

        private void OnEnable()
        {
            GameManager.Instance.InteractibleManager.RegisterInteractible(this);
        }

        public abstract void StartInteraction(Entity entity);
        public abstract void FinishInteraction(Entity entity);
        public abstract string GetName();
        public Floor GetFloor()
        {
            var col = GetComponent<Collider2D>();
            List<Collider2D> overlap = new List<Collider2D>();
            col.OverlapCollider(new ContactFilter2D(), overlap);
            var floor = overlap.FindAll(c => c.GetComponent<Floor>() != null).Select(c => c.GetComponent<Floor>()).First();
            return floor;
        }

        public abstract string GetTooltip();
    }
}