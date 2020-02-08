namespace Core
{
    public class OfficeDoor : Interactible
    {
        public NPC Owner;

        public override void FinishInteraction(Entity entity)
        {
            
        }

        public override string GetName() => Owner.Name + "s Büro";
        public override string GetTooltip() => "";
    }
}