using System;
using Core;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI TextComponent;
        private IHasToolTip _lastDisplayed;
        
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
            var offsetY = 0f;
            var collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                offsetY = collider.bounds.extents.y;
            }

            return new Vector2(x, y - offsetY);
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

            TextComponent.text = text;

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