using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
        public SongHandler SongHandler;
        public AudioClip BeamerSound;
        public AudioClip BeamerClick;


        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void PlayBeamerClick()
        {
            SoundEffectAudioSource.PlayOneShot(BeamerClick, 2f);
        }

        public void PlayAmbientBeamerSound()
        {
            SoundEffectAudioSource.clip = BeamerSound;
            SoundEffectAudioSource.Play();
        }

        public void StopSound()
        {
            SoundEffectAudioSource.Stop();
        }
        
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

        public void ToggleMuteAll()
        {
            ToggleMuteEffects();
            ToggleMuteMusic();
        }
        
        public void ToggleMuteEffects(bool state = false)
        {
            SoundEffectAudioSource.mute = !SoundEffectAudioSource.mute;
        }

        public void ToggleMuteMusic(bool state = false)
        {
            if (SongHandler == null) SongHandler = FindObjectOfType<SongHandler>();
            
            // ReSharper disable once Unity.NoNullPropagation
            SongHandler?.ToggleMute();
        }
    }
}