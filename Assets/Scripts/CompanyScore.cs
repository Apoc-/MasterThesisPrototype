using System.Collections.Generic;
using System.Linq;

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

    public void AddEffect(string effectDescription, int value)
    {
        Value += value;
        if (Value <= 0) Value = 0;
            
        if (Effects.Count(effect => effect.Description == effectDescription) > 0)
        {
            var existingEffect = Effects.First(effect => effect.Description == effectDescription);
            existingEffect.Value += value;
        }
        else
        {
            var newEffect = new CompanyScoreEffect(effectDescription, value);
            Effects.Add(newEffect);
        }
    }
        
    public void ClearEffectTab()
    {
        Effects = new List<CompanyScoreEffect>();
    }
}