using UnityEngine;

namespace Core
{
    public class Chair : Interactible
    {
        public override void StartInteraction(Entity entity)
        {
            (entity as Player)?.StartWork();
        }

        public override void FinishInteraction(Entity entity)
        {
            (entity as Player)?.StopWork();
        }

        public override string GetName() => "Stuhl";
        public override string GetTooltip() => "Arbeiten";
    }
}