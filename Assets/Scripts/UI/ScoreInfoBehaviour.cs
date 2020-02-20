using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreInfoBehaviour : ScreenBehaviour
    {
        public Image Background;
        public GameObject MailIcon;

        public void OnEnable()
        {
            MailIcon.GetComponent<Jun_TweenRuntime>().enabled = true;
        }

        public void FinishReport()
        {
            Hide();
            GameManager.Instance.InitNewDay();
        }
    }
}