using System;
using System.Collections.Generic;
using System.Linq;
using Tech;
using TMPro;
using UnityEngine;

namespace UI
{
    public class AdvisorScreenBehaviour : MonoBehaviour
    {
        public DialogueBehaviour DialogueBox;
        public HashSet<Plan> ActivatedPlans = new HashSet<Plan>();
        public PlanInfoBehaviour PlanInfo;
        public ScoreInfoBehaviour ScoreInfo;

        public void DisplayPlanInfo(Plan plan, Action callback)
        {
            PlanInfo.SetTextForPlan(plan, callback);
            PlanInfo.Show();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}