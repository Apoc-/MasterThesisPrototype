using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Fixable : Interactible
    {
        public bool IsBroken { get; private set; } = false;
        private GameObject _warningSign;

        public void Fix()
        {
            Debug.Log("Fixed " + GetName());
            Destroy(_warningSign);
            IsBroken = false;
            OnFixed();
        }

        public void Break()
        {
            if(IsBroken) return;
            var pref = Resources.Load("Prefabs/WarningSign");
            _warningSign = Instantiate(pref, transform) as GameObject;
            _warningSign.transform.localPosition = Vector3.zero;
            IsBroken = true;

            Debug.Log("Broke " + GetName());
        }
        
        public override void FinishInteraction()
        {
            if(IsBroken) Fix();
        }

        public abstract void OnFixed();
    }
}