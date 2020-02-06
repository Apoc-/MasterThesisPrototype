using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ReportTextBehaviour : ScreenBehaviour
    {
        [SerializeField] private TextMeshProUGUI _reportTextField;
        
        private void OnEnable()
        {
            var scores = GameManager.Instance.Company.CompanyScores;
            var scoreText = "";
            
            scores.ForEach(score =>
            {
                scoreText += "<b>" + score.Name + "</b>\n";
                
                score.Effects.ForEach(effect =>
                {
                    var val = effect.Value;
                    var effectText = "";
                    if (Math.Sign(val) > 0)
                    {
                        effectText += "+";
                    }
                    else
                    {
                        effectText += "-";
                    }

                    effectText += val;
                    effectText += " (" + effect.Description + ")\n";
                    scoreText += effectText;
                });
                
                score.ClearEffectTab();
                scoreText += "Summe: " + score.Value + "\n\n";
            });

            _reportTextField.text = scoreText;
        }
    }
}