using System;
using Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Toggle))]
    public class EffectMuteButton : MonoBehaviour
    {
        private void OnEnable()
        {
            UnityAction<bool> func = SoundEffectManager.Instance.ToggleMuteEffects;
            var toggle = GetComponent<Toggle>();
            toggle.isOn = SoundEffectManager.Instance.SoundEffectAudioSource.mute;
            toggle.onValueChanged.AddListener(func);
        }
    }
}