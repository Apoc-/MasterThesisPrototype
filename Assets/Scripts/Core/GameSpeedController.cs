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

        private enum Speed
        {
            Play,
            Fast
        }
        
        public void Play()
        {
            PlayFlash.gameObject.SetActive(true);
            PlayFlash.Play();
            _lastSpeed = Speed.Play;
            Time.timeScale = 1;
            ResetColors();
            PlayButton.color = PressedColor;
        }
        
        public void PlayFast()
        {
            PlayFastFlash.gameObject.SetActive(true);
            PlayFastFlash.Play();
            _lastSpeed = Speed.Fast;
            Time.timeScale = 32;
            ResetColors();
            PlayFastButton.color = PressedColor;
        }
        
        public void Pause()
        {
            PauseFlash.gameObject.SetActive(true);
            PauseFlash.Play();
            Time.timeScale = 0;
            ResetColors();
            PauseButton.color = PressedColor;
        }

        public void UnPause()
        {
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