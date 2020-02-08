using System;

namespace Core
{
    public class MeetingWarningSign : Interactible
    {
        public NPC AttachedNPC;
        
        public override void FinishInteraction(Entity entity)
        {
            GameManager.Instance.AddToAgility("Scrum Master Tätigkeiten", 1);
            AttachedNPC.GoToMeeting();
            Destroy(gameObject);    
        }

        public override string GetName() => "Zu Meeting schicken";
    }
}