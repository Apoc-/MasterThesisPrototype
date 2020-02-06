using System;
using DefaultNamespace;
using Tech;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI CompanyScoreDisplay;
        public AdvisorScreenBehaviour AdvisorScreen;
        
        public void UpdateCompanyScores()
        {
            var comp = GameManager.Instance.Company;
            var text = "";
            
            comp.CompanyScores.ForEach(score =>
            {
                text += score.Name + ": " + score.Value + "\n";
            });
            
            CompanyScoreDisplay.text = text;
        }
        
        #region Singleton
        private static UiManager _instance;
        public static UiManager Instance => _instance ? _instance : _instance = FindObjectOfType<UiManager>();
        #endregion

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(plan), plan, null);
            }
        }

        private void Update()
        {
            UpdateCompanyScores();
        }
    }
}