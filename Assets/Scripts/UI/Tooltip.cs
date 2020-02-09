using System;
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
            var x = Input.mousePosition.x - 64;
            var y = Input.mousePosition.y + 16;
            return new Vector2(x, y);
        }

        public void Show(IHasToolTip hasToolTip)
        {
            if (isActiveAndEnabled) return;
            if (_lastDisplayed != hasToolTip)
            {
                var text = hasToolTip.GetTooltip();
                if (text == "") return;
            
                _textComponent.text = text;
            }
            
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