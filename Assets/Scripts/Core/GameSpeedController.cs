using System;
using UnityEditor;
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

        public Jun_TweenRuntime PauseFlash;
        public Jun_TweenRuntime PlayFlash;
        public Jun_TweenRuntime PlayFastFlash;
        
        private Speed _lastSpeed = Speed.Play;

        public bool IsPaused = false;
        
        private enum Speed
        {
            Play,
            Fast
        }
        
        public void Play()
        {
            IsPaused = false;
            DisableAllFlashes();
            PlayFlash.gameObject.SetActive(true);
            PlayFlash.Play();
            _lastSpeed = Speed.Play;
            Time.timeScale = 1;
            ResetColors();
            PlayButton.color = PressedColor;
        }
        
        public void PlayFast()
        {
            IsPaused = false;
            DisableAllFlashes();
            PlayFastFlash.gameObject.SetActive(true);
            PlayFastFlash.Play();
            _lastSpeed = Speed.Fast;
            Time.timeScale = 32;
            ResetColors();
            PlayFastButton.color = PressedColor;
        }
        
        public void Pause()
        {
            if (IsPaused) return;
            
            IsPaused = true;
            DisableAllFlashes();
            PauseFlash.gameObject.SetActive(true);
            PauseFlash.Play();
            Time.timeScale = 0;
            ResetColors();
            PauseButton.color = PressedColor;
        }

        private void DisableAllFlashes()
        {
            PlayFlash.gameObject.SetActive(false);
            PauseFlash.gameObject.SetActive(false);
            PlayFastFlash.gameObject.SetActive(false);
        }


        public void UnPause()
        {
            if (!IsPaused) return;
            
            switch (_lastSpeed)
            {
                case Speed.Play:
                    Play();
                    break;
                case Speed.Fast:
                    PlayFast();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ResetColors()
        {
            PlayButton.color = BaseColor;
            PlayFastButton.color = BaseColor;
            PauseButton.color = BaseColor;
        }
    }
}