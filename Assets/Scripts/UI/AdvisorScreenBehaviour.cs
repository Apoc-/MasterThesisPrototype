using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Tech;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Collider2D))]
    public class AdvisorScreenBehaviour : ScreenBehaviour
    {
        public DialogueBehaviour DialogueBox;
        public HashSet<Plan> ActivatedPlans = new HashSet<Plan>();
        public PlanInfoBehaviour PlanInfo;
        public ScoreInfoBehaviour ScoreInfo;

        public void OnEnable()
        {
            GameManager.Instance.SongHandler?.PlaySongById(3);
        }

        public void DisplayPlanInfo(Plan plan, Action callback)
        {
            PlanInfo.SetTextForPlan(plan, callback);
            PlanInfo.Show();
        }
    }
}