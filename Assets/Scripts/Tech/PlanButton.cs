using System;
using UnityEngine;

namespace Tech
{
    public class PlanButton : MonoBehaviour
    {
        private Action _clickAction;
        
        public void ExecuteClickAction()
        {
            _clickAction?.Invoke();
        }

        public void SetClickAction(Action action)
        {
            _clickAction = action;
        }
    }
}