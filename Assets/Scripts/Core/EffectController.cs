using System;
using UI;
using UnityEngine;

namespace Core
{
    public class EffectController : MonoBehaviour
    {
        public Effect EffectPrefab;

        public void PlayPositiveEffectAt(Vector2 pos, Vector2 target)
        {
            var eff = Instantiate(EffectPrefab, transform, true);
            eff.transform.position = pos;

            var dir = target - pos;
            eff.Velocity = new Vector2(0,0) ;
            eff.Acceleration = dir;
            eff.Alive = 5;
        }
    }
}