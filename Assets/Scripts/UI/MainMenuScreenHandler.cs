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

        public GameObject AnyKeyTextObject;

        private bool _logoAnimPlayed = false;
        
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
    }
}