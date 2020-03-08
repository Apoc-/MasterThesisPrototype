using System;
using System.Collections.Generic;
using System.Linq;
using Tasklist;
using TMPro;
using UI;
using UnityEngine;

namespace Core
{
    public class TasklistScreenBehaviour : ScreenBehaviour
    {
        public TextMeshProUGUI BonusText;

        public GameObject ImpedimentsContainer;
        public ImpedimentEntry ImpedimentEntryPrefab;

        private BonusTask CurrentBonusTask;
        private float _bonusPullMaxTime = 30;
        private float _bonusPullTimer = 0;
        
        private Dictionary<Interactible, ImpedimentEntry> _impedimentEntries = new Dictionary<Interactible, ImpedimentEntry>();

        private void Start()
        {
            UpdateBonusTaskText();
        }

        private void Update()
        {
            _bonusPullTimer -= Time.deltaTime;
            if (_bonusPullTimer <= 0 && CurrentBonusTask == null)
            {
                if (!BonusTaskProvider.HasTasksQueued()) return;
                
                CurrentBonusTask = BonusTaskProvider.GetNextBonusTask();
                CurrentBonusTask.Start();
                UpdateBonusTaskText();
            }

            if (_bonusPullTimer <= 0)
            {
                _bonusPullTimer = _bonusPullMaxTime;
            }
        }

        public void ReportTaskProgress(BonusTaskType type, int amount = 1)
        {
            if (CurrentBonusTask == null) return;
            if (CurrentBonusTask.Type != type) return;

            CurrentBonusTask.CurrentCount += amount;
            
            if (CurrentBonusTask.CurrentCount >= CurrentBonusTask.MaxCount)
            {
                GameManager.Instance.AddToAgility(
                    "Bonus Aufgabe erledigt", 
                    CurrentBonusTask.Bonus, 
                    GameManager.Instance.player.OverheadPosition);

                CurrentBonusTask = null;
            }

            UpdateBonusTaskText();
        }

        private void UpdateBonusTaskText()
        {
            var text = "";

            if (CurrentBonusTask != null)
            {
                text += $"<b>+{CurrentBonusTask.Bonus} Agilität: </b>";
                text += CurrentBonusTask.Text;
                text += $" ({CurrentBonusTask.CurrentCount}/{CurrentBonusTask.MaxCount})";
            }
            else
            {
                text += "Aktuell keine Bonusaufgabe";
            }
            
            BonusText.text = text;
        }

        public void AddImpediment(Interactible interactible, bool IsUnique = false)
        {
            if (IsUnique)
            {
                if (_impedimentEntries.ContainsKey(interactible)) return;
            }
            
            var entry = Instantiate(ImpedimentEntryPrefab, ImpedimentsContainer.transform);
            //entry.transform.localPosition = Vector3.zero;
            entry.TextElement.text = interactible.GetTooltip();

            _impedimentEntries[interactible] = entry;
        }

        public void RemoveImpediment(Interactible interactible)
        {
            if (!_impedimentEntries.ContainsKey(interactible)) return;
            
            var entry = _impedimentEntries[interactible];
            _impedimentEntries.Remove(interactible);
            Destroy(entry.gameObject);
        }
    }
}