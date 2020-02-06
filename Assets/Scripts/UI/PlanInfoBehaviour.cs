using System;
using Tech;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlanInfoBehaviour : ScreenBehaviour
    {
        private Action _okCallback;

        public void OnOkClick()
        {
            _okCallback?.Invoke();
            Hide();
        }

        public void SetTextForPlan(Plan plan, Action callback)
        {
            _okCallback = callback;
            var text = Resources.Load("Plans/" + plan) as TextAsset;

            if (text != null)
            {
                GetComponentInChildren<TextMeshProUGUI>().text = text.text;
                Show();
            }
        }
    }
}