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

        public bool _stopped = false;
        
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        
        private void OnEnable()
        {
            AudioListener.volume = 0.5f;
        }

        public void StopPlaying()
        {
            _stopped = true;
            AudioSource.Stop();
        }

        public void StartPlaying()
        {
            _stopped = false;
        }

        private void Update()
        {
            if (!AudioSource.isPlaying && !_stopped)
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
            AudioSource.mute = !AudioSource.mute;
        }

        public void PlaySongById(int id)
        {
            var song = Songs[id];

            AudioSource.clip = song;
            AudioSource.Play();
        }
    }
}