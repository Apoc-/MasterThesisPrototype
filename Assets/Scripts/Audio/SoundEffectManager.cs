using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class SoundEffectManager : MonoBehaviour
    {
        #region Singleton
        private static SoundEffectManager _instance;
        public static SoundEffectManager Instance 
            => _instance 
                ? _instance 
                : _instance = FindObjectOfType<SoundEffectManager>();
        #endregion
        
        public List<AudioClip> Pops;
        public AudioClip Dud;
        public AudioClip Door;
        public AudioSource SoundEffectAudioSource;

        public Toggle MuteToggle;
        
        private bool _isMuted = false;

        public void PlayDoorSound()
        {
            SoundEffectAudioSource.PlayOneShot(Door,0.25f);
        }
        
        public void PlayRandomPop()
        {
            var rnd = Random.Range(0, Pops.Count);
            
            SoundEffectAudioSource.PlayOneShot(Pops[rnd]);
        }
        
        public void PlayDud()
        {
            SoundEffectAudioSource.PlayOneShot(Dud, 2);
        }

        public void ToggleMute()
        {
            _isMuted = !_isMuted;
            AudioListener.volume = _isMuted ? 0 : 0.5f;
        }

        private void OnEnable()
        {
            if (AudioListener.volume <= 0)
            {
                MuteToggle.isOn = true;
                AudioListener.volume = 0;
            }
            else
            {
                MuteToggle.isOn = false;
                AudioListener.volume = 0.5f;
            }
        }
    }
}