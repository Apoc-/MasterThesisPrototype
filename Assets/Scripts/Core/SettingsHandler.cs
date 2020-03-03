using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Core
{
    public class SettingsHandler : MonoBehaviour
    {
        public UniversalRenderPipelineAsset LowQualityAsset;
        public UniversalRenderPipelineAsset HighQualityAsset;

        private bool _isLow = true;

        public GameObject LightsContainer;

        private List<Light2D> _lights;

        public void OnEnable()
        {
            _lights = LightsContainer.GetComponentsInChildren<Light2D>().ToList();
            TurnOffLights();
        }

        public void ToggleQuality()
        {
            _isLow = !_isLow;

            if (_isLow)
            {
                TurnOffLights();
            }
            else
            {
                TurnOnLights();
            }
            
            GraphicsSettings.renderPipelineAsset = _isLow ? LowQualityAsset : HighQualityAsset;
        }

        private void TurnOnLights()
        {
            _lights.ForEach(light => light.gameObject.SetActive(true));
        }

        private void TurnOffLights()
        {
            _lights.ForEach(light => light.gameObject.SetActive(false));
        }
    }
}