using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Toggle))]
    public class MusicMuteButton : MonoBehaviour
    {
        private void OnEnable()
        {
            UnityAction<bool> func = SoundEffectManager.Instance.ToggleMuteMusic;
            var toggle = GetComponent<Toggle>();
            toggle.isOn = SoundEffectManager.Instance.SongHandler.AudioSource.mute;
            toggle.onValueChanged.AddListener(func);
        }
    }
}