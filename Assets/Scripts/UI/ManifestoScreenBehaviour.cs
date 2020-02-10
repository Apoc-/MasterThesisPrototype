﻿using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class ManifestoScreenBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI Quote;
        public TextMeshProUGUI Sign;
        
        private List<string> _manifesto;

        public void OnEnable()
        {
            if(_manifesto == null) LoadManifesto();
            
            var r = Random.Range(0, _manifesto.Count);
            Quote.text = _manifesto[r];
            Sign.text = "Agile Principle #" + (r + 1);
        }

        private void LoadManifesto()
        {
            _manifesto = new List<string>();
            var manifestoTextAsset = Resources.Load("manifesto") as TextAsset;

            _manifesto = manifestoTextAsset.text
                .Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None)
                .ToList();
        }
        
        
    }
}