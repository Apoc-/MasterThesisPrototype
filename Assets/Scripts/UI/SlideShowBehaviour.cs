using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SlideShowBehaviour : MonoBehaviour
    {
        public GameObject SlideContainer;

        public List<GameObject> Slides;

        public GameObject FadeImage;
        public GameObject Glow;

        public int CurrentSlide = -1;

        public List<Image> PlayerImages;
        public Sprite Lucy;
        public Sprite Jack;
        
        public AudioClip ClickSound;
        private SoundEffectManager _soundEffectManager;
        public SoundEffectManager SoundEffectManager => _soundEffectManager
            ? _soundEffectManager
            : _soundEffectManager = FindObjectOfType<SoundEffectManager>();

        private void OnEnable()
        {
            Slides = GetSlides();
            HideAllSlides();
            ShowFirstSlide();

            var avatarId = FindObjectOfType<PregameScreenBehaviour>().SettingHandler.AvatarId;
            PlayerImages.ForEach(image =>
            {
                image.sprite = (avatarId == 0) ? Lucy : Jack;
            });
            
            if (SoundEffectManager != null)
            {
                SoundEffectManager.PlayAmbientBeamerSound();
                SoundEffectManager.SongHandler.EnableRadioEffect();
            }
        }

        private void OnDisable()
        {
            if (SoundEffectManager != null)
            {
                SoundEffectManager.StopSound();
                SoundEffectManager.SongHandler.DisableRadioEffect();    
            }
        }

        private void ShowFirstSlide()
        {
            CurrentSlide = 0;
            ShowSlideById(CurrentSlide);
        }

        private void ShowSlideById(int id)
        {
            HideAllSlides();
            Slides[id].SetActive(true);
        }

        private List<GameObject> GetSlides()
        {
            var children = new List<GameObject>();
            for (var i = 0; i < SlideContainer.transform.childCount; i++)
            {
                children.Add(SlideContainer.transform.GetChild(i).gameObject);
            }

            return children;
        }

        private void HideAllSlides()
        {
            Slides.ForEach(slide => slide.SetActive(false));
        }

        public void ShowNextSlide()
        {
            if (CurrentSlide >= Slides.Count-1)
            {
                StartGame();
                return;
            }
            
            if (SoundEffectManager != null)
            {
                SoundEffectManager.PlayBeamerClick();    
            }
            
            Glow.SetActive(false);
            FadeImage.SetActive(true);
            var jun = FadeImage.GetComponent<Jun_TweenRuntime>();
            jun.AddOnFinished(() => { Glow.SetActive(true); });
            jun.Play();
            CurrentSlide += 1;
            ShowSlideById(CurrentSlide);
        }
        
        public void StartGame()
        {
            
            SceneManager.LoadScene("Scenes/GameScene");
        }

    }
}