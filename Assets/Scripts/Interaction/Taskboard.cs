using System;
using UI;

namespace Core
{
    public class Taskboard : Interactible
    {
        public TaskBoardScreen TaskBoardScreen;
        
        public override void StartInteraction(Entity entity)
        {
            TaskBoardScreen.gameObject.SetActive(true);
            GameManager.Instance.player.CanGiveMoveCommand = false;
        }

        public override void FinishInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Taskboard";
        public override string GetTooltip() => "Taskboard prüfen";
    }
}