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

        private int _wrongTaskCount = 0;
        private float GetWrongTaskTimer() => (_wrongTaskCount != 0) ? 4f/_wrongTaskCount : 1f;
        private float _currentWrongTaskTimer = 1;
        
        public override void StartInteraction(Entity entity)
        {
            if (entity is NPC npc)
            {
                HandleNpcInteraction(npc);
            }
            else
            {
                TaskBoardScreen.gameObject.SetActive(true);
                GameManager.Instance.player.DisableCommands();
            }
        }

        public void Update()
        {
            if (_wrongTaskCount <= 0) return;

            _currentWrongTaskTimer -= Time.deltaTime;
            if(_currentWrongTaskTimer <= 0) 
            {
                GameManager.Instance.AddToTeamspirit(
                    "Unordnung auf dem Taskboard", 
                    -_wrongTaskCount, 
                    transform.position);
                
                _currentWrongTaskTimer = GetWrongTaskTimer();
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
            CheckWrongTasks();
        }

        public void CheckWrongTasks()
        {
            _wrongTaskCount = TaskBoardScreen.Tasks.Count(task => !task.IsInCorrectLane());
            
            if (_wrongTaskCount > 0)
            {
                DisplayWarning();
                GameManager.Instance.TasklistScreenBehaviour.AddImpediment(this, true);
            }
            else
            {
                RemoveWarning();
                GameManager.Instance.TasklistScreenBehaviour.RemoveImpediment(this);
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
        public override string GetTooltip() => "Taskboard in Ordnung bringen";
    }
}