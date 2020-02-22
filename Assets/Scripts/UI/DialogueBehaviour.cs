using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using Tech;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DialogueBehaviour : ScreenBehaviour
    {
        public GameObject NextButton;
        public GameObject PlanButtonContainer;
        public GameObject PlanButtonPrefab;

        private Dictionary<string, Dialogue> _dialogues = new Dictionary<string, Dialogue>();
        private Dialogue _currentDialogue;
        public TextMeshProUGUI _textField;

        public void Awake()
        {
            LoadDialoguesFromFile();
        }

        private void OnEnable()
        {
            
        }

        private void LoadDialoguesFromFile()
        {
            _dialogues = new Dictionary<string, Dialogue>();
            var dialogueDataTextAsset = Resources.Load("Dialogues/dialogues") as TextAsset;
            var dialogueData = dialogueDataTextAsset.text.Split('\n');

            dialogueData.ToList().ForEach(date =>
            {
                var trimmedDate = date.Trim();
                var content = Resources.Load("Dialogues/" + trimmedDate) as TextAsset;
                var lines = content.text.Split('\n').ToList();
                var dia = new Dialogue(lines);

                _dialogues[trimmedDate] = dia;
            });
        }

        public void ShowDialogueById(string id)
        {
            if (_dialogues.Count == 0) LoadDialoguesFromFile();

            RemovePlanButtons();
            
            _currentDialogue = _dialogues[id];
            Show();
            NextButton.SetActive(true);
            NextLine();
        }

        private void RemovePlanButtons()
        {
            while (PlanButtonContainer.transform.childCount > 0)
            {
                var child = PlanButtonContainer.transform.GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }

        public void NextLine()
        {
            var line = _currentDialogue.GetNextLine();
            if (line == null) return;

            if (line.StartsWith("$"))
            {
                var commands = line.Substring(1).Split('|');
                
                foreach (var command in commands)
                {
                    var token = command.Trim().Split(':');
                    ParseCommand(token);
                }
            } 
            else
            {
                line = ReplaceVariables(line);
                _textField.text = line;
            }
        }

        //todo quick solution, needs more efficient one in the future
        private string ReplaceVariables(string line)
        {
            var replacedLine = line;
            replacedLine = replacedLine.Replace("{progress}",
                ""+GameManager.Instance.Company.CompanyScores.First(score => score.Name == "Fortschritt").Value);
            
            return replacedLine;
        }

        private void ParseCommand(string[] token)
        {
            var cmd = token[0].Trim();
            
            switch (cmd)
            {
                case "plan":
                {
                    var param = token[1].Trim();
                    AddPlanButton((Plan) Enum.Parse(typeof(Plan), param));
                    NextButton.SetActive(false);
                }
                    break;
                case "end_game":
                {
                    GameManager.Instance.FinishGame();
                    break;
                }
            }
        }

        public void AddPlanButton(Plan plan)
        {
            var btn = Instantiate(PlanButtonPrefab, PlanButtonContainer.transform);
            btn.name = plan.ToString();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = PlanButtonTextProvider.GetButtonTextByPlan(plan);

            var planButton = btn.GetComponent<PlanButton>();

            void PlanInfoClickedAction()
            {
                UiManager.Instance.AdvisorScreen.Hide();
                UiManager.Instance.ActivatePlan(plan);
                UiManager.Instance.ManifestoScreen.FinishedCallback = GameManager.Instance.StartDay;
                UiManager.Instance.ManifestoScreen.Show();
            }

            void PlanButtonClickedAction()
            {
                UiManager.Instance.AdvisorScreen.DisplayPlanInfo(plan, PlanInfoClickedAction);
            }

            planButton.SetClickAction(PlanButtonClickedAction);
        }
    }
}