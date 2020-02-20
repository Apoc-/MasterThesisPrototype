using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class ManifestoScreenBehaviour : ScreenBehaviour
    {
        public TextMeshProUGUI Quote;
        public TextMeshProUGUI Sign;
        
        private List<string> _manifesto;
        private List<string> _manifestoAvailable;

        public void OnEnable()
        {
            if(_manifestoAvailable == null) LoadManifesto();
            
            var r = Random.Range(0, _manifestoAvailable.Count);
            Quote.text = _manifestoAvailable[r];
            Sign.text = "Agile Principle #" + (r + 1);
            _manifestoAvailable.RemoveAt(r);
            
            GetComponentsInChildren<Jun_TweenRuntime>().ToList().ForEach(tween => tween.enabled = true);
        }
        
        private void LoadManifesto()
        {
            _manifesto = new List<string>();
            
            var manifestoTextAsset = Resources.Load("manifesto") as TextAsset;

            _manifesto = manifestoTextAsset.text
                .Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None)
                .ToList();
            
            _manifestoAvailable = _manifesto.ToList();
        }
        
        
    }
}