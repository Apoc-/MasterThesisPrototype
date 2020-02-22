using System.Collections.Generic;

namespace Tech
{
    public static class PlanButtonTextProvider
    {
        private static readonly Dictionary<Plan, string> ButtonTexts = new Dictionary<Plan, string>
        {
            {Plan.SCRUM_MASTER, "Scrum Master einführen"},
            {Plan.DAILY_SCRUM, "Daily Scrum Meeting einführen"},
            {Plan.TASK_BOARD, "Taskboard einführen"}
            
        };
        
        public static string GetButtonTextByPlan(Plan plan) => ButtonTexts[plan];
    }
}