using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MeetingRoomBehaviour : MonoBehaviour
    {
        public bool HasMeeting = false;
        public string CurrentMeetingName;
        
        private bool _meetingIsRunning = false;
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
                member.CallToMeeting();
                _invitedEntities.Add(member);
            });
        }

        public void EnterMeeting(Entity entity)
        {
            entity.Hide();
            _arrivedEntities.Add(entity);
        }

        public void StartMeeting()
        {

            if (_arrivedEntities.Count != _invitedEntities.Count)
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Das Daily-Scrum-Meeting hat begonnnen, jedoch schwänz irgendjemand!",
                        NotificationType.Warning);
            }
            else
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification("Das Daily-Scrum-Meeting hat begonnen.", NotificationType.Default);
            }
        }
        
        public void StopMeeting()
        {
            if (_arrivedEntities.Count == _invitedEntities.Count)
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Das Daily-Scrum-Meeting ist fertig und jeder war da! Ja man, das gibt extra Punkte!",
                        NotificationType.Default);
                
                GameManager.Instance.AddToAgility("Daily Scrum: Volles Haus!", 10, transform.position);       
            }
            else
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Mist! Das Daily-Scrum-Meeting ist fertig und jemand hat gefehlt! Das kostet Punkte!",
                        NotificationType.Warning);
                
                GameManager.Instance.AddToAgility("Daily Scrum: Jemand war nicht beim Daily!", -10, transform.position);
            }
            
            _invitedEntities.ForEach(entity =>
            {
                entity.CancelCurrentInteractionOrder();
                entity.CancelCurrentWalkOrder();
                entity.ReturnFromMeeting();
            });

            HasMeeting = false;
            CurrentMeetingName = "Kein Meeting";
            Debug.Log("Stopping Meeting");
        }
    }
}