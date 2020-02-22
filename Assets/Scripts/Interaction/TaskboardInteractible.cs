using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class TaskboardInteractible : Interactible
    {
        public TaskBoardScreen TaskBoardScreen;
        public float MistakeChance = 0.1f;

        public GameObject LightContainer;
        public GameObject Stuff;
        
        private GameObject _warningSign;
        
        public override void StartInteraction(Entity entity)
        {
            if (entity is NPC npc)
            {
                HandleNpcInteraction(npc);
            }
            else
            {
                TaskBoardScreen.gameObject.SetActive(true);
                GameManager.Instance.player.CanGiveCommand = false;
            }
        }

        private void HandleNpcInteraction(NPC npc)
        {
            var tasksNotDone = TaskBoardScreen.Tasks
                .Where(task => task.Owner == npc 
                    && task.CurrentLane != TaskBoardScreen.DoneLane 
                    || task.CurrentLane == TaskBoardScreen.TodoLane)
                .ToList();
                
            TaskBehaviour handledTask;

            if (!tasksNotDone.Any())
            {
                handledTask = TaskBoardScreen.CreateNewTask();
            }
            else
            {
                handledTask = tasksNotDone.First();
            }
            
            TaskBoardScreen.ProgressTask(handledTask, npc);

            MakeRandomMistake(handledTask);
        }

        private void MakeRandomMistake(TaskBehaviour handledTask)
        {
            var madeMistake = false;
            if (handledTask.CurrentLane.laneType == TaskboardLaneType.DONE)
            {
                var testedRnd = Random.Range(0f, 1f);
                if (testedRnd < MistakeChance)
                {
                    handledTask.IsTested = false;
                    madeMistake = true;
                }
                
                var documentedRnd = Random.Range(0f, 1f);
                if (documentedRnd < MistakeChance)
                {
                    handledTask.IsDocumented = false;
                    madeMistake = true;
                }
            }
            else if(handledTask.CurrentLane.laneType == TaskboardLaneType.DOING)
            {
                var rnd = Random.Range(0f, 1f);
                if (rnd < MistakeChance)
                {
                    handledTask.StartTime = "";
                    handledTask.IsStarted = false;
                    madeMistake = true;
                }
                
                rnd = Random.Range(0f, 1f);
                if (rnd < MistakeChance)
                {
                    handledTask.Owner = null;
                    handledTask.IsStarted = false;
                    madeMistake = true;
                }
            }
            
            if (madeMistake)
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Auf dem Taskboard ist etwas nicht richtig eingeordnet.", 
                        NotificationType.Warning);
            }
        }

        public override void FinishInteraction(Entity entity)
        {
            if (TaskBoardScreen.Tasks.Any(task => !task.IsInCorrectLane()))
            {
                DisplayWarning();       
            }
            else
            {
                RemoveWarning();
            }
        }

        private void DisplayWarning()
        {
            if (_warningSign != null) return;
            
            var pref = Resources.Load("Prefabs/WarningSign");
            _warningSign = Instantiate(pref, transform) as GameObject;
            _warningSign.transform.localPosition = Vector3.zero;
        }

        private void RemoveWarning()
        {
            if (_warningSign == null) return;
            _warningSign.gameObject.SetActive(false);
            Destroy(_warningSign.gameObject);
        }
        
        public override string GetName() => "Taskboard";
        public override string GetTooltip() => "Taskboard prüfen";
    }
}