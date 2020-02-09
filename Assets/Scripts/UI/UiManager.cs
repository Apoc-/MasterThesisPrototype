using System;
using System.Linq;
using Core;
using Tech;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI CompanyScoreDisplay;
        public AdvisorScreenBehaviour AdvisorScreen;
        public TaskBoardScreen TaskBoardScreen;
        public Tooltip Tooltip;
        public Tooltip LargeTooltip;

        public void UpdateCompanyScores()
        {
            var comp = GameManager.Instance.Company;
            var text = "";

            comp.CompanyScores.ForEach(score => { text += score.Name + ": " + score.Value + "\n"; });

            CompanyScoreDisplay.text = text;
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
            Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero, 0f);

            if (hits.Length == 0) return;

            var tooltipObjects = hits
                .Where(hit => hit.collider.gameObject.GetComponent<IHasToolTip>() != null)
                .ToList();

            if (tooltipObjects.Any())
            {
                var go = tooltipObjects.First().collider.gameObject;
                var hitObject = go.GetComponent<IHasToolTip>();
                Tooltip.Show(hitObject);
            }
            else
            {
                Tooltip.Hide();
            }
        }

        #region Singleton

        private static UiManager _instance;
        public static UiManager Instance => _instance ? _instance : _instance = FindObjectOfType<UiManager>();

        #endregion
    }
}