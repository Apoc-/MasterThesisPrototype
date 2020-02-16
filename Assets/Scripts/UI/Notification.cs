using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Notification : MonoBehaviour
    {
        public TextMeshProUGUI TextComponent;
        private float _timeToLive = 4f;
        private bool _dismissing = false;
        public NotificationController Controller { get; set; }

        public void Update()
        {
            _timeToLive -= Time.unscaledDeltaTime;

            if (_timeToLive <= 0 && !_dismissing)
            {
                Dismiss();
            }
        }

        public void SetText(string text)
        {
            TextComponent.text = text;
        }

        public void Dismiss()
        {
            _dismissing = true;
            var tween = GetComponent<Jun_TweenRuntime>();
            
            tween.Rewind();
            tween.AddOnFinished(() =>
            {
                Controller.RemoveNotification(this);
                gameObject.SetActive(false);
                Destroy(gameObject);
            });
        }
    }
}