using TMPro;

namespace UI
{
    public class ModalBehaviour : ScreenBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Text;

        private void OnEnable()
        {
            
            GetComponent<Jun_TweenRuntime>().Play();
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