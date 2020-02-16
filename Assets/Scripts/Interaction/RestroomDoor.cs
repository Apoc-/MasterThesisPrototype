namespace Core
{
    public class RestroomDoor : Interactible
    {
        public override void StartInteraction(Entity entity)
        {
            entity.Hide();
        }

        public override void FinishInteraction(Entity entity)
        {
            entity.Show();
        }

        public override string GetName() => "WC Tür";

        public override string GetTooltip() => "";
    }
}