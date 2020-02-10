using System;
using UI;
using UnityEngine;

namespace Core
{
    public class EffectController : MonoBehaviour
    {
        public Effect PlusEffectPrefab;
        public Effect MinusEffectPrefab;
        
        public void PlayPlusEffectAt(Vector2 pos, Vector2 target, Action callback)
        {
            PlayEffectAt(pos, target, PlusEffectPrefab, callback);
        }
        
        public void PlayMinusEffectAt(Vector2 pos, Vector2 target, Action callback)
        {
            PlayEffectAt(pos, target, MinusEffectPrefab, callback);
        }

        private void PlayEffectAt(Vector2 pos, Vector2 target, Effect effectPrefab, Action targetReachedCallback)
        {
            var effect = Instantiate(effectPrefab, transform, true);
            effect.transform.position = pos;

            var dir = target - pos;
            effect.Velocity = new Vector2(0,0) ;
            effect.Acceleration = dir;
            effect.Target = target;
            effect.TargetReachedCallback = targetReachedCallback;
        }
    }
}