namespace Core
{
    public class OfficeDoor : Interactible
    {
        public NPC Owner;

        public override void FinishInteraction()
        {
            
        }

        public override string GetName() => Owner.Name + "'s Office";
    }
}