using UnityEngine;

namespace Core
{
    public class Chair : Interactible
    {
        public override void StartInteraction(Entity entity)
        {
            
        }

        public override void FinishInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Stuhl";
        public override string GetTooltip() => "Hinsetzen";
    }
}