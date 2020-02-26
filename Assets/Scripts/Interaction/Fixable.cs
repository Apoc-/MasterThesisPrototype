using System;
using System.Collections.Generic;
using Tasklist;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Fixable : Interactible
    {
        public bool IsBroken { get; private set; } = false;
        protected List<string> _brokenTooltips = new List<string> {
            "Ich bin Groot!"
        };
        
        private string _currentBrokenTooltip = "";
        
        private GameObject _warningSign;

        public void Fix(Entity entity)
        {
            Destroy(_warningSign);
            IsBroken = false;
            GameManager.Instance.AddToAgility("Scrum Master Tätigkeiten", 1, entity.OverheadPosition);
            _currentBrokenTooltip = "";
            GameManager.Instance.TasklistScreenBehaviour.RemoveImpediment(this);
            GameManager.Instance.TasklistScreenBehaviour.ReportTaskProgress(BonusTaskType.Todo);
            OnFixed();
        }

        public void Break()
        {
            if(IsBroken) return;
            SetBrokenTooltip();
            var pref = Resources.Load("Prefabs/WarningSign");
            _warningSign = Instantiate(pref, transform) as GameObject;
            _warningSign.transform.localPosition = Vector3.zero;
            IsBroken = true;

            GameManager.Instance.NotificationController.DisplayNotification(GetBrokenMessage(), NotificationType.Warning);
            GameManager.Instance.TasklistScreenBehaviour.AddImpediment(this);
        }

        private void SetBrokenTooltip()
        {
            _currentBrokenTooltip = _brokenTooltips[Random.Range(0, _brokenTooltips.Count)];
        }

        public override void StartInteraction(Entity entity)
        {
            
        }

        public override void FinishInteraction(Entity entity)
        {
            if (IsBroken) HandleIsBroken(entity);
            if (!IsBroken) HandleNotBroken(entity);
        }

        private void HandleNotBroken(Entity entity)
        {
            if (entity is NPC)
            {
                GameManager.Instance.AddToTeamspirit("Hindernisfreies Arbeiten", 3, entity.OverheadPosition);
            }
        }

        private void HandleIsBroken(Entity entity)
        {
            if (entity is Player)
            {
                Fix(entity);
            }

            if (entity is NPC)
            {
                GameManager.Instance.AddToTeamspirit("Hindernisse beim Arbeiten", -1, entity.OverheadPosition);
            }
        }
        public abstract void OnFixed();
        public override string GetTooltip() => _currentBrokenTooltip;

        public virtual string GetBrokenMessage() => "Irgendetwas hindert das Team daran optimal zu arbeiten!";

    }
}