using System;
using TMPro;

namespace UI
{
    public class InfoModalBehaviour : ModalBehaviour
    {
        public Action OkAction;

        public void OnOkClick()
        {
            Hide();
            OkAction?.Invoke();
        }
    }
}