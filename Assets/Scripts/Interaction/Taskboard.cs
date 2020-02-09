using System;
using UI;

namespace Core
{
    public class Taskboard : Interactible
    {
        public TaskBoardScreen TaskBoardScreen;

        private void Start()
        {
            InteractionDuration = 0.1f;
        }

        public override void FinishInteraction(Entity entity)
        {
            TaskBoardScreen.gameObject.SetActive(true);
            GameManager.Instance.player.CanGiveMoveCommand = false;
        }

        public override string GetName() => "Taskboard";
        public override string GetTooltip() => "Taskboard prüfen";
    }
}