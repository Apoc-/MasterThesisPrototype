using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using UI;

namespace DefaultNamespace
{
    public class CompanyScoreEffect
    {
        public string Description;
        public int Value;

        public CompanyScoreEffect(string description, int value)
        {
            Description = description;
            Value = value;
        }
    }
    
    public class CompanyScore
    {
        public string Name;
        public int Value;
        public List<CompanyScoreEffect> Effects = new List<CompanyScoreEffect>();

        public CompanyScore(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public void AggregateEffects()
        {
            Effects.ForEach(effect => { Value += effect.Value; });
            Effects = new List<CompanyScoreEffect>();
        }
    }
    
    public class Company
    {
        public string Name;
        public List<CompanyScore> CompanyScores = new List<CompanyScore>();

        public Company(string name)
        {
            Name = name;
            var agility = new CompanyScore("Agilität", 0);
            var teamspirit = new CompanyScore("Teamgeist", 50);
            
            CompanyScores.Add(agility);
            CompanyScores.Add(teamspirit);
        }

        public void AddEffectToCompanyScore(string scoreName, string effectDescription, int value)
        {
            var effect = new CompanyScoreEffect(effectDescription, value);
            CompanyScores.First(score => score.Name == scoreName).Effects.Add(effect);   
        }
    }
}