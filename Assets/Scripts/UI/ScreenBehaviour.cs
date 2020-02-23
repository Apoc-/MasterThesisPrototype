using UnityEngine;

namespace UI
{
    public class ScreenBehaviour : MonoBehaviour
    {
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