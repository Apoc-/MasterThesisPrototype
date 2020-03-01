using System;
using System.Linq;
using Core;
using Tech;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI AgilityScore;
        public TextMeshProUGUI TeamspiritScore;
        public TextMeshProUGUI ProgressScore;
        public AdvisorScreenBehaviour AdvisorScreen;
        public TaskBoardScreen TaskBoardScreen;
        public WikiScreenBehaviour ScrumWikiScreen;
        public FinishGameScreen FinishGameScreen;
        public ManifestoScreenBehaviour ManifestoScreen;
        public Tooltip Tooltip;
        public Tooltip LargeTooltip;
        public DecisionModalBehaviour DecisionModal;
        public InfoModalBehaviour InfoModal;

        public void DisplayGameQuitModal()
        {
            DecisionModal.SetTitle("Spiel Beenden");
            DecisionModal.SetText("Möchtest du das Spiel wirklich verlassen?");
            DecisionModal.NoAction = () => { GameManager.Instance.GameSpeedController.UnPause(); };
            DecisionModal.YesAction = Application.Quit;

            DecisionModal.Show();
        }
        
        public void UpdateCompanyScores()
        {
            var comp = GameManager.Instance.Company;

            var agi = comp.CompanyScores.Find(score => score.Name == "Agilität");
            var spi = comp.CompanyScores.Find(score => score.Name == "Produktivität");
            var prog = comp.CompanyScores.Find(score => score.Name == "Fortschritt");

            AgilityScore.text = agi.Name + " " + agi.Value;
            TeamspiritScore.text = spi.Name + " " + spi.Value;
            ProgressScore.text = "" + prog.Value;
        }


        public void ShowScoreScreen()
        {
            AdvisorScreen.DialogueBox.Hide();
            AdvisorScreen.PlanInfo.Hide();
            AdvisorScreen.ScoreInfo.Show();
            AdvisorScreen.Show();
        }

        public bool PlanIsActive(Plan plan)
        {
            return AdvisorScreen.ActivatedPlans.Contains(plan);
        }

        public void ActivatePlan(Plan plan)
        {
            AdvisorScreen.ActivatedPlans.Add(plan);

            switch (plan)
            {
                case Plan.SCRUM_MASTER:
                    GameManager.Instance.InitScrumMasterPlan();
                    break;
                case Plan.DAILY_SCRUM:
                    GameManager.Instance.InitDailyScrumPlan();
                    break;
                case Plan.TASK_BOARD:
                    GameManager.Instance.InitTaskBoardPlan();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(plan), plan, null);
            }
        }

        private void Update()
        {
            UpdateCompanyScores();
            CheckMouseOver();
        }

        private void CheckMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Tooltip.Hide();
                return;
            }

            Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero, 0f);

            if (hits.Length == 0) return;
            
            var tooltipObjects = hits
                .Where(hit => hit.collider.gameObject.GetComponent<IHasToolTip>() != null)
                .ToList();

            if (tooltipObjects.Any())
            {
                foreach (var obj in tooltipObjects)
                {
                    var tooltip = obj.collider.gameObject.GetComponent<IHasToolTip>();
                    if (tooltip.GetTooltip() != "")
                    {
                        Tooltip.Show(tooltip);
                        break;
                    }
                    else
                    {
                        Tooltip.Hide();
                    }
                }
            }
            else
            {
                Tooltip.Hide();
            }
        }

        public void HideAllScreens()
        {
            ScrumWikiScreen.Hide();
            AdvisorScreen.Hide();
            TaskBoardScreen.Hide();
        }

        #region Singleton

        private static UiManager _instance;
        public static UiManager Instance => _instance ? _instance : _instance = FindObjectOfType<UiManager>();

        #endregion
    }
}