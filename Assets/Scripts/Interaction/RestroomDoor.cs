namespace Core
{
    public class RestroomDoor : Interactible
    {
        public override void StartInteraction(Entity entity)
        {
            if (entity is NPC)
            {
                entity.Hide();    
            }
        }

        public override void FinishInteraction(Entity entity)
        {
            if (entity is NPC)
            {
                entity.Show();    
            }
        }

        public override string GetName() => "WC Tür";

        public override string GetTooltip() => "";
    }
}