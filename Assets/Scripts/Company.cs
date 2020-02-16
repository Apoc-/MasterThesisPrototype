using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Core;
using UI;
public class Company
{
    public string Name;
    public List<CompanyScore> CompanyScores = new List<CompanyScore>();
    public List<Entity> Team = new List<Entity>();

    public Company(string name)
    {
        Name = name;
        var agility = new CompanyScore("Agilität", 0);
        var teamspirit = new CompanyScore("Teamgeist", 50);
        var progress = new CompanyScore("Fortschritt", 0);
        
        CompanyScores.Add(agility);
        CompanyScores.Add(teamspirit);
        CompanyScores.Add(progress);
    }
        
    public void AddEffectToCompanyScore(string scoreName, string effectDescription, int value)
    {
        var targetScore = CompanyScores.First(score => score.Name == scoreName);
        targetScore.AddEffect(effectDescription, value);
    }

    public void RegisterTeamMember(Entity entity)
    {
        Team.Add(entity);
    }

    public float GetProgressTimer()
    {
        var agi = CompanyScores.First(score => score.Name == "Agilität").Value;
        var tsp = CompanyScores.First(score => score.Name == "Teamgeist").Value;

        return 12000f / (agi*agi + tsp*tsp);
    }
}