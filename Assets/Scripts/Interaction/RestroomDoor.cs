using UI;

namespace Core
{
    public class RestroomDoor : Interactible
    {
        public override void StartInteraction(Entity entity)
        {
            if (entity is NPC)
            {
                entity.Hide();
                SoundEffectManager.Instance.PlayDoorSound();
            }
        }

        public override void FinishInteraction(Entity entity)
        {
            if (entity is NPC)
            {
                entity.Show();
                SoundEffectManager.Instance.PlayDoorSound();
            }
        }

        public override string GetName() => "WC Tür";

        public override string GetTooltip() => "";
    }
}