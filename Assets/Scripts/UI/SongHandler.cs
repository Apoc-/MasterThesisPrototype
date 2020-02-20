using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class SongHandler : MonoBehaviour
    {
        public List<AudioClip> Songs;
        public AudioSource AudioSource;

        private bool _isMuted = false;

        private void OnEnable()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            if (!AudioSource.isPlaying)
            {
                PlayRandomSong();
            }
        }

        private void PlayRandomSong()
        {
            var rnd = Random.Range(0, Songs.Count);
            PlaySongById(rnd);
        }

        public void ToggleMute()
        {
            _isMuted = !_isMuted;
            if (_isMuted)
            {
                AudioSource.volume = 0;    
            }
            else
            {
                AudioSource.volume = 0.05f;  
            }
            
        }

        public void PlaySongById(int id)
        {
            var song = Songs[id];

            AudioSource.clip = song;
            AudioSource.Play();
        }
    }
}