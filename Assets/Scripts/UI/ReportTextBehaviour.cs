using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ReportTextBehaviour : ScreenBehaviour
    {
        public TextMeshProUGUI ReportTextField;

        public void ShowManifesto()
        {
            var manifesto = UiManager.Instance.ManifestoScreen;
            manifesto.FinishedCallback = UiManager.Instance.AdvisorScreen.ScoreInfo.FinishReport;

            manifesto.gameObject.SetActive(true);
        }
        
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

                    effectText += val;
                    effectText += " (" + effect.Description + ")\n";
                    scoreText += effectText;
                });
                
                score.ClearEffectTab();
                scoreText += "Neuer Wert: " + score.Value + "\n\n";
            });

            ReportTextField.text = scoreText;
        }
    }
}