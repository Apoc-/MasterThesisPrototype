using System;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DecisionModalBehaviour : ModalBehaviour
    {
        public Action YesAction;
        public Action NoAction;

        private void OnEnable()
        {
            GameManager.Instance.GameSpeedController.ForcePause();
        }

        public void OnYesClick()
        {
            Hide();
            YesAction?.Invoke();
        }

        public void OnNoClick()
        {
            Hide();
            NoAction?.Invoke();
        }
    }
}