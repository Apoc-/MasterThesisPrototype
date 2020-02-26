using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Core;
using Tasklist;
using UI;
using UnityEngine;

public class Company
{
    public string Name;
    public List<CompanyScore> CompanyScores = new List<CompanyScore>();
    public List<Entity> Team = new List<Entity>();

    public Company(string name)
    {
        Name = name;
        var agility = new CompanyScore("Agilität", 0);
        var teamspirit = new CompanyScore("Produktivität", 50);
        var progress = new CompanyScore("Fortschritt", 0);
        
        CompanyScores.Add(agility);
        CompanyScores.Add(teamspirit);
        CompanyScores.Add(progress);
    }
        
    public void AddEffectToCompanyScore(string scoreName, string effectDescription, int value)
    {
        var targetScore = CompanyScores.First(score => score.Name == scoreName);
        targetScore.AddEffect(effectDescription, value);

        if (scoreName == "Fortschritt")
        {
            for (int i = 0; i < value; i++)
            {
                GameManager.Instance.TasklistScreenBehaviour.ReportTaskProgress(BonusTaskType.ReachProgress);    
            }
        }
    }

    public void RegisterTeamMember(Entity entity)
    {
        Team.Add(entity);
    }

    public float GetProgressTimer()
    {
        var agi = CompanyScores.First(score => score.Name == "Agilität").Value;
        var tsp = CompanyScores.First(score => score.Name == "Produktivität").Value;

        var timer = 40000f / (agi * agi + tsp * tsp);
        timer = Mathf.Clamp(timer, 0.25f, 2f);
        
        return timer;
    }
}