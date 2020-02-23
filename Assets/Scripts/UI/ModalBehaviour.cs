using TMPro;

namespace UI
{
    public class ModalBehaviour : ScreenBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Text;
        
        private void OnEnable()
        {
            GameManager.Instance.GameSpeedController.Pause();
            GetComponent<Jun_TweenRuntime>().Play();
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