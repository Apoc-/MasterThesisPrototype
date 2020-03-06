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
        public float BaseInteractionDuration = 3;
        public float PlayerInteractDurationFactor = 0.5f;
        private IHasToolTip _hasToolTipImplementation;
        public int InteractionLayer = 0;
        public Collider2D InteractionClickCollider;
        
        public void OnEnable()
        {
            GameManager.Instance.InteractibleManager.RegisterInteractible(this);
            InteractionClickCollider = GetComponent<Collider2D>();
        }

        public abstract void StartInteraction(Entity entity);
        public abstract void FinishInteraction(Entity entity);
        public abstract string GetName();
        public Floor GetFloor()
        {
            List<Collider2D> overlap = new List<Collider2D>();
            InteractionClickCollider.OverlapCollider(new ContactFilter2D(), overlap);
            var floor = overlap.FindAll(c => c.GetComponent<Floor>() != null).Select(c => c.GetComponent<Floor>()).First();
            return floor;
        }

        public float GetInteractDuration(Entity entity)
        {
            if (entity is NPC)
            {
                return BaseInteractionDuration;
            }
            
            return BaseInteractionDuration * PlayerInteractDurationFactor;
        }

        public virtual Vector3 GetNPCWalkTarget()
        {
            return transform.position;
        }

        public abstract string GetTooltip();
    }
}