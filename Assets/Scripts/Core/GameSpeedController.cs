﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core
{
    public class GameSpeedController : MonoBehaviour
    {
        public Color BaseColor;
        public Color PressedColor;

        public Image DebugButton;
        public Image PlayButton;
        public Image PlayFastButton;
        public Image PauseButton;

        public Jun_TweenRuntime PauseFlash;
        public Jun_TweenRuntime PlayFlash;
        public Jun_TweenRuntime PlayFastFlash;
        
        private Speed _lastSpeed = Speed.Play;

        public bool IsPaused = false;

        public float BaseTimeScale = 1f;

        private bool _canUnpause = true;
        
        private enum Speed
        {
            Play,
            Fast,
            Debug
        }

        private void Start()
        {
            if (GameManager.Instance.IsDebugMode)
            {
                DebugButton.gameObject.SetActive(true);
            }
            else
            {
                DebugButton.gameObject.SetActive(false);
            }
        }

        public void DebugSpeed()
        {
            if (!_canUnpause) return;
            if (_lastSpeed == Speed.Debug && !IsPaused) return;
            
            IsPaused = false;
            _lastSpeed = Speed.Debug;
            Time.timeScale = 16 * BaseTimeScale;
            ResetColors();
            DebugButton.color = PressedColor;
        }

        public void Play()
        {
            if (!_canUnpause) return;
            if (_lastSpeed == Speed.Play && !IsPaused) return;
            
            IsPaused = false;
            DisableAllFlashes();
            PlayFlash.gameObject.SetActive(true);
            PlayFlash.Play();
            _lastSpeed = Speed.Play;
            Time.timeScale = 1 * BaseTimeScale;
            ResetColors();
            PlayButton.color = PressedColor;
        }
        
        public void PlayFast()
        {
            if (!_canUnpause) return;
            if (_lastSpeed == Speed.Fast && !IsPaused) return;
            
            IsPaused = false;
            DisableAllFlashes();
            PlayFastFlash.gameObject.SetActive(true);
            PlayFastFlash.Play();
            _lastSpeed = Speed.Fast;
            Time.timeScale = 2 * BaseTimeScale;
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

        public void ForcePause()
        {
            Pause();
            _canUnpause = false;
        }

        public void ForceUnPause()
        {
            _canUnpause = true;
            UnPause();
        }

        public void UnPause()
        {
            if (!_canUnpause) return;
            if (!IsPaused) return;
            
            switch (_lastSpeed)
            {
                case Speed.Play:
                    Play();
                    break;
                case Speed.Fast:
                    PlayFast();
                    break;
                case Speed.Debug:
                    DebugSpeed();
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
            DebugButton.color = BaseColor;
        }
    }
}