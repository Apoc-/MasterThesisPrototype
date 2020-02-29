using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuScreenHandler : ScreenBehaviour
    {
        public Jun_TweenRuntime FadeIn;
        public Jun_TweenRuntime FadeOut;
        public Jun_TweenRuntime CameraAnim;
        public Camera BackgroundCamera;
        public Jun_TweenRuntime LogoAnim;
        private Vector3 CameraStart;

        public Jun_TweenRuntime Credits;
        
        public GameObject AnyKeyTextObject;

        private bool _logoAnimPlayed = false;
        private bool _creditsPlaying = false;
        
        
        private void Awake()
        {
            CameraStart = BackgroundCamera.transform.position;
        }

        private void Update()
        {
            if (Input.anyKeyDown && _logoAnimPlayed == false)
            {
                StartLogoAnim();
            }

            if (Input.anyKeyDown && _creditsPlaying)
            {
                StopCredits();
            }
        }

        private void StopCredits()
        {
            _creditsPlaying = false;
            Credits.transform.parent.gameObject.SetActive(false);
            Credits.StopPlay();
        }

        private void StartLogoAnim()
        {
            LogoAnim.Play();
            _logoAnimPlayed = true;
            AnyKeyTextObject.SetActive(false);
        }

        public void LoadPregameScene()
        {
            SceneManager.LoadScene("Scenes/PregameScene");
        }

        public void EndGame()
        {
            Application.Quit(0);
        }
        
        public void ResetCamera()
        {
            BackgroundCamera.transform.position = CameraStart;
            FadeOut.gameObject.SetActive(true);
            FadeIn.gameObject.SetActive(false);
            
            FadeOut.Play();
            FadeOut.AddOnFinished(() => CameraAnim.Play());
        }

        public void PlayCredits()
        {
            _creditsPlaying = true;
            Credits.transform.parent.gameObject.SetActive(true);
            Credits.Play();
        }
    }
}