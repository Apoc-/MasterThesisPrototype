using Core;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Collider2D))]
    public class DoorSignBehaviour : MonoBehaviour, IHasToolTip
    {
        public string TootltipText;

        public string GetTooltip() => TootltipText;
    }
}