using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textComponent;
        private float TimeToLive = 4f;
        private bool _dismissing = false;
        public NotificationController Controller { get; set; }

        public void Update()
        {
            TimeToLive -= Time.deltaTime;

            if (TimeToLive <= 0 && !_dismissing)
            {
                Dismiss();
            }
        }

        public void SetText(string text)
        {
            _textComponent.text = text;
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