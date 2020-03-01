using System;
using TMPro;

namespace UI
{
    public class InfoModalBehaviour : ModalBehaviour
    {
        public Action OkAction;

        private void OnDisable()
        {
            GameManager.Instance.GameSpeedController.ForceUnPause();
        }
        
        public void OnOkClick()
        {
            Hide();
            OkAction?.Invoke();
        }
    }
}