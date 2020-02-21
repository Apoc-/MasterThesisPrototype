using System;
using System.Collections.Generic;
using System.Transactions;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class Phone : Interactible
    {
        public GameObject RingingEffect;
        private bool _isRinging;
        private int _currentCallId = 0;

        public void Ring(int id)
        {
            _currentCallId = id;
            _isRinging = true;
            RingingEffect.SetActive(true);
            
            GameManager.Instance.NotificationController
                .DisplayNotification(
                    "Das Telefon klingelt sturm, muss wohl was ganz wichtiges sein!",
                    NotificationType.Warning);
        }

        public void StopRinging()
        {
            _isRinging = false;
            RingingEffect.SetActive(false);
        }

        public void Answer(int id)
        {
            _isRinging = false;
            RingingEffect.SetActive(false);
            TriggerDecisionModalById(id);
        }
        
        public override void StartInteraction(Entity entity)
        {
            
        }

        public override void FinishInteraction(Entity entity)
        {
            if (_isRinging)
            {
                Answer(_currentCallId);    
            }
        }

        public override string GetName() => "Telefon";
        public override string GetTooltip() => _isRinging ? "Anruf annehmen" : "";
        private void TriggerDecisionModalById(int id)
        {
            var text = Resources.Load<TextAsset>("PhoneCalls/call"+id);
            var modal = UiManager.Instance.DecisionModal;
            var lines = text.text.Split('\n');
            var answerA = true;
            var modalText = "";
            
            foreach (var line in lines)
            {
                if (line.StartsWith("$answer"))
                {
                    var ans = line.Split(':')[1];
                    answerA = ans == "yes";
                }
                else
                {
                    modalText += line + "\n";
                }
            }
            
            modal.SetTitle("Anruf");
            modal.SetText(modalText);
            
            modal.YesAction = () =>
            {
                if (answerA) 
                {
                    HandleCorrectAnswer(id);
                }
                else
                {
                    HandleWrongAnswer(id);
                }
            };
            
            modal.NoAction = () =>
            {
                if (answerA) 
                {
                    HandleWrongAnswer(id);
                }
                else
                {
                    HandleCorrectAnswer(id);
                }
            };
            
            modal.Show();
        }

        private void HandleCorrectAnswer(int id)
        {
            var correctPrefix = "Gute Entscheidung: +10 Agilität\n\n";
            
            GameManager.Instance.AddToAgility(
                "Richtige Entscheidung", 
                10, 
                gameObject.transform.position);
                    
            TriggerInfoModalById(id, correctPrefix);
        }
        
        private void HandleWrongAnswer(int id)
        {
            var wrongPrefix = "Falsche Entscheidung: -10 Agilität\n\n";
            
            GameManager.Instance.AddToAgility(
                "Falsche Entscheidung", 
                -10, 
                gameObject.transform.position);

                    
            TriggerInfoModalById(id, wrongPrefix);
        }

        private void TriggerInfoModalById(int id, string prefix)
        {
            var text = Resources.Load<TextAsset>("PhoneCalls/answer"+id);
            var modal = UiManager.Instance.InfoModal;
            
            modal.SetTitle("Information");
            modal.SetText(prefix + text.text);
            modal.Show();
        }
    }
}