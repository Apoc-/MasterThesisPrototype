using UnityEngine;

namespace UI
{
    public class FinishGameScreen : ScreenBehaviour
    {
        public void OnFinishGameButtonClicked()
        {
            Application.Quit(0);
        }
    }
}