using System;
using UnityEngine;

namespace UI
{
    public class ScreenBehaviour : MonoBehaviour
    {
        protected bool _isQuitting = false;
        
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}