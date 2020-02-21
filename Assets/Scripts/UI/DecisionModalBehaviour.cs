using System;
using TMPro;

namespace UI
{
    public class DecisionModalBehaviour : ModalBehaviour
    {
        public Action YesAction;
        public Action NoAction;
        
        public void OnYesClick()
        {
            Hide();
            YesAction?.Invoke();
        }

        public void OnNoClick()
        {
            Hide();
            NoAction?.Invoke();
        }
    }
}