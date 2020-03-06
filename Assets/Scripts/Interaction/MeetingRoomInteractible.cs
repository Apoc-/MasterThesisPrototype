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

        public List<GameObject> MeetingStandingLocations;
        private int _occupiedStandingLocations = 0;
        
        public GameObject TargetGraphicContainer;
        public GameObject LucyOutline;
        public GameObject JackOutline;
        
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

        private void InvitePlayerToMeeting()
        {
            var player = GameManager.Instance.player;
            player.CallToMeeting(this);
            _invitedEntities.Add(player);

            EnableTargetGraphics();
        }

        private void EnableTargetGraphics()
        {
            //todo hacky, the player avatar can only be one or the other, needs refactoring
            LucyOutline.SetActive(GameManager.Instance.SettingHandler.AvatarId == 0);
            JackOutline.SetActive(GameManager.Instance.SettingHandler.AvatarId == 1);
            
            TargetGraphicContainer.SetActive(true);
        }
        
        private void DisableTargetGraphics()
        {
            TargetGraphicContainer.SetActive(false);
        }

        public void EnterMeeting(Entity entity)
        {
            _arrivedEntities.Add(entity);
            
            if (entity is Player player)
            {
                player.EnterMeeting();
                DisableTargetGraphics();
            }
            else
            {
                if (_arrivedEntities.Count >= _invitedEntities.Count)
                {
                    InvitePlayerToMeeting();
                }
            }
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
                        "Das Daily-Scrum-Meeting ist abgeschlossen und alle waren da. Super!",
                        NotificationType.Default);

                GameManager.Instance.AddToAgility("Daily Scrum: Alle waren anwesend!", 10, transform.position);
            }
            else
            {
                GameManager.Instance
                    .NotificationController
                    .DisplayNotification(
                        "Mist! Das Daily-Scrum-Meeting ist fertig und es waren nicht alle da.",
                        NotificationType.Warning);

                GameManager.Instance.AddToAgility("Daily Scrum: Jemand war nicht beim Daily!", -10, transform.position);
            }

            _invitedEntities.ForEach(entity =>
            {
                entity.CancelCurrentInteractionOrder();
                entity.CancelCurrentWalkOrder();
                entity.ReturnFromMeeting();
                
            });
            
            DisableTargetGraphics();
            _occupiedStandingLocations = 0;

            HasMeeting = false;
            CurrentMeetingName = "Kein Meeting";
        }

        public override void StartInteraction(Entity entity)
        {
            if (!HasMeeting) return;
            
            EnterMeeting(entity);
        }

        public override Vector3 GetNPCWalkTarget()
        {
            var target = MeetingStandingLocations[_occupiedStandingLocations].transform.position;
            _occupiedStandingLocations += 1;
            return target;
        }

        public override void FinishInteraction(Entity entity)
        {
        }

        public override string GetName() => "Meeting Raum";

        public override string GetTooltip() => TargetGraphicContainer.activeSelf ? "An Meeting teilnehmen." : "";
    }
}