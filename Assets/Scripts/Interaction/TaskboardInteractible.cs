using System;
using System.Linq;
using UI;

namespace Core
{
    public class TaskboardInteractible : Interactible
    {
        public TaskBoardScreen TaskBoardScreen;

        public override void StartInteraction(Entity entity)
        {
            if (entity is NPC npc)
            {
                HandleNpcInteraction(npc);
            }
            else
            {
                TaskBoardScreen.gameObject.SetActive(true);
                GameManager.Instance.player.CanGiveMoveCommand = false;
            }
        }

        private void HandleNpcInteraction(NPC npc)
        {
            var tasksNotDone = TaskBoardScreen.Tasks
                .Where(task => task.Owner == npc && task.CurrentLane != TaskBoardScreen.DoneLane)
                .ToList();
                
            TaskBehaviour handledTask;

            if (!tasksNotDone.Any())
            {
                handledTask = TaskBoardScreen.CreateNewTask(npc);
            }
            else
            {
                handledTask = tasksNotDone.First();
            }
            
            TaskBoardScreen.ProgressTask(handledTask);
            
            if (handledTask.CurrentLane == TaskBoardScreen.DoneLane)
            {
                TaskBoardScreen.CreateNewTask(npc);
            }
        }

        public override void FinishInteraction(Entity entity)
        {
        }

        public override string GetName() => "Taskboard";
        public override string GetTooltip() => "Taskboard prüfen";
    }
}