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
            GameManager.Instance.AddToAgility("Scrum Master Tätigkeiten", 1);
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
        
        public override void FinishInteraction(Entity entity)
        {
            if (IsBroken) HandleIsBroken(entity);
            if (!IsBroken) HandleNotBroken(entity);
        }

        private void HandleNotBroken(Entity entity)
        {
            if (entity is NPC)
            {
                GameManager.Instance.AddToTeamspirit("Hindernisfreies arbeiten", +1);
            }
        }

        private void HandleIsBroken(Entity entity)
        {
            if (entity is Player)
            {
                Fix();
            }

            if (entity is NPC)
            {
                GameManager.Instance.AddToTeamspirit("Hindernisse beim Arbeiten", -1);
            }
        }

        public abstract void OnFixed();
    }
}