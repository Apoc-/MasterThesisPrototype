using System;
using System.Collections.Generic;
using Tasklist;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class HelpWarningSign : Interactible
    {
        public NPC AttachedNPC;

        private string _helpTooltip;
        
        private void Awake()
        {
            var tooltips = new List<string>
            {
                "'s Scrum-Frage beantworten",
                " agiles Konzept erklären",
                " das Vorgehen erläutern",
                " Zugangsdaten geben",
                "'s Unklarheit beseitigen"
            };

            _helpTooltip = tooltips[Random.Range(0, tooltips.Count)];
        }

        public override void StartInteraction(Entity entity)
        {
            if (AttachedNPC.IsInOffice)
            {
                (entity as Player)?.Hide();
                (entity as Player)?.DisableOutline();  
            }
        }

        public override void FinishInteraction(Entity entity)
        {
            GameManager.Instance.AddToAgility("Scrum Master Tätigkeiten", 3, entity.OverheadPosition);
            GameManager.Instance.TasklistScreenBehaviour.RemoveImpediment(this);
            GameManager.Instance.TasklistScreenBehaviour.ReportTaskProgress(BonusTaskType.Todo);

            if (this == null) return; //ee if already destroyed
            
            AttachedNPC.NeedsHelp = false;
         
            (entity as Player)?.Show();
            (entity as Player)?.EnableOutline();
            Destroy(gameObject);
        }

        public override string GetName() => "Hilfe Warnung";
        public override string GetTooltip() => AttachedNPC.Name + _helpTooltip;
    }
}