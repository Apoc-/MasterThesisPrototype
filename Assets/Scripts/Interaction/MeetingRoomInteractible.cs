using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class MeetingRoomInteractible : Interactible
    {
        public bool HasMeeting = false;
        public string CurrentMeetingName;
        
        private List<Entity> _invitedEntities = new List<Entity>();
        private List<Entity> _arrivedEntities = new List<Entity>();

        public void CallForMeeting(string name)
        {
            CurrentMeetingName = name;
            HasMeeting = true;
            _invitedEntities.Clear();
            _arrivedEntities.Clear();

            var team = GameManager.Instance.Company.Team;
            team.ForEach(member =>
            {
                member.CallToMeeting(this);
                _invitedEntities.Add(member);
            });
        }

        public void EnterMeeting(Entity entity)
        {
            entity.Hide();

            (entity as Player)?.EnterMeeting();
            
            _arrivedEntities.Add(entity);
        }

        public void StartMeeting()
        {
            GameManager.Instance
                    .NotificationController
                    .DisplayNotification("Das Daily-Scrum-Meeting hat begonnen.", NotificationType.Default);
        }

        public void StopMeeting()
        {
            if (_arrivedEntities.Count >= _invitedEntities.Count)
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Das Daily-Scrum-Meeting ist abgeschlossen und alle Entwickler waren da. Super!",
                        NotificationType.Default);

                GameManager.Instance.AddToAgility("Daily Scrum: Alle Entwickler anwesend!", 10, transform.position);
            }
            else
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Mist! Das Daily-Scrum-Meeting ist fertig und es waren nicht alle Entwickler da.",
                        NotificationType.Warning);

                GameManager.Instance.AddToAgility("Daily Scrum: Einer der Entwickler war nicht beim Daily!", -10, transform.position);
            }

            _invitedEntities.ForEach(entity =>
            {
                entity.CancelCurrentInteractionOrder();
                entity.CancelCurrentWalkOrder();
                entity.ReturnFromMeeting();
            });
            
            GameManager.Instance.player.ReturnFromMeeting();

            HasMeeting = false;
            CurrentMeetingName = "Kein Meeting";
            Debug.Log("Stopping Meeting");
        }

        public override void StartInteraction(Entity entity)
        {
            var meetingRoomBehaviour = GameManager.Instance.MeetingRoomInteractible;
            if (!meetingRoomBehaviour.HasMeeting) return;

            EnterMeeting(entity);
        }

        public override void FinishInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Meeting Raum" ;

        public override string GetTooltip() => "";

        /*var pos = SpriteRenderer.bounds.center;
            var sign = Instantiate(
                Resources.Load<GameObject>("Prefabs/MeetingWarningSign"),
                transform);*/
    }
}