using System;
using Tasklist;
using UnityEngine;

namespace Core
{
    public class MeetingWarningSign : Interactible
    {
        public NPC AttachedNPC;
        public MeetingRoomInteractible MeetingRoomInteractible;
        
        public override void StartInteraction(Entity entity)
        {
            
        }

        public override void FinishInteraction(Entity entity)
        {
            GameManager.Instance.AddToAgility("Scrum Master Tätigkeiten", 3, entity.OverheadPosition);
            GameManager.Instance.TasklistScreenBehaviour.RemoveImpediment(this);
            AttachedNPC.GiveGoToMeetingCommand(MeetingRoomInteractible);
            Destroy(gameObject);
        }

        public override string GetName() => "Meeting Warnung";
        public override string GetTooltip() => AttachedNPC.Name + " zu Meeting schicken";
    }
}