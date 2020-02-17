using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class GameSpeedController : MonoBehaviour
    {
        public Color BaseColor;
        public Color PressedColor;

        public Image PlayButton;
        public Image PlayFastButton;
        public Image PauseButton;
        
        public void Play()
        {
            Time.timeScale = 1;
            ResetColors();
            PlayButton.color = PressedColor;
        }
        
        public void PlayFast()
        {
            Time.timeScale = 32;
            ResetColors();
            PlayFastButton.color = PressedColor;
        }
        
        public void Pause()
        {
            Time.timeScale = 0;
            ResetColors();
            PauseButton.color = PressedColor;
        }

        private void ResetColors()
        {
            PlayButton.color = BaseColor;
            PlayFastButton.color = BaseColor;
            PauseButton.color = BaseColor;
        }
    }
}