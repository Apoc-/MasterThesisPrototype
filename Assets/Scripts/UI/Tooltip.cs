﻿using System;
using Core;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textComponent;
        private IHasToolTip _lastDisplayed;

        private void Start()
        {
            //gameObject.SetActive(false);
        }

        private void Update()
        {
            if (isActiveAndEnabled)
            {
                transform.position = CalcTooltipPosition();
            }
        }

        private Vector2 CalcTooltipPosition()
        {
            var x = Input.mousePosition.x;
            var y = Input.mousePosition.y;
            return new Vector2(x, y);
        }

        public void Show(IHasToolTip hasToolTip)
        {
            if (isActiveAndEnabled) return;
            
            var text = hasToolTip.GetTooltip();
            if (text == "")
            {
                Hide();
                return;
            }

            _textComponent.text = text;

            transform.position = CalcTooltipPosition();
            gameObject.SetActive(true);
            _lastDisplayed = hasToolTip;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}