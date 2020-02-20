using System;
using TMPro;

namespace UI
{
    public class ModalDialogueBehaviour : ScreenBehaviour
    {
        public Action YesAction;
        public Action NoAction;

        public TextMeshProUGUI Title;
        public TextMeshProUGUI Text;

        public void OnYesClick()
        {
            YesAction?.Invoke();
            Hide();
        }

        public void OnNoClick()
        {
            NoAction?.Invoke();
            Hide();
        }
        
        private void OnEnable()
        {
            GameManager.Instance.GameSpeedController.Pause();
        }

        private void OnDisable()
        {
            GameManager.Instance.GameSpeedController.UnPause();
        }

        public void SetTitle(string title)
        {
            Title.text = title;
        }

        public void SetText(string text)
        {
            Text.text = text;
        }
    }
}