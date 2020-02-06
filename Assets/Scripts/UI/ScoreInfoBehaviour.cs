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

        public void OnReportClick()
        {
            Hide();
            GameManager.Instance.InitNewDay();
        }
    }
}