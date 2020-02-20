using System.Collections.Generic;
using UnityEngine;

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
        public AudioSource SoundEffectAudioSource;

        private bool _isMuted = false;
        
        public void PlayRandomPop()
        {
            var rnd = Random.Range(0, Pops.Count);
            
            SoundEffectAudioSource.PlayOneShot(Pops[rnd]);
        }

        public void ToggleMute()
        {
            _isMuted = !_isMuted;
            AudioListener.volume = _isMuted ? 0 : 1;
        }
    }
}