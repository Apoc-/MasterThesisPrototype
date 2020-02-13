using System;
using UI;
using UnityEngine;

namespace Core
{
    public class EffectController : MonoBehaviour
    {
        public Effect AgilityPlusPrefab;
        public Effect AgilityMinusPrefab;
        public Effect TeamspiritPlusPrefab;
        public Effect TeamspiritMinusPrefab;
        
        public void PlayTsPlusEffectAt(Vector2 pos, Vector2 target, Action callback)
        {
            PlayEffectAt(pos, target, TeamspiritPlusPrefab, callback);
        }
        
        public void PlayTsMinusEffectAt(Vector2 pos, Vector2 target, Action callback)
        {
            PlayEffectAt(pos, target, TeamspiritMinusPrefab, callback);
        }
        
        public void PlayAgiPlusEffectAt(Vector2 pos, Vector2 target, Action callback)
        {
            PlayEffectAt(pos, target, AgilityPlusPrefab, callback);
        }
        
        public void PlayAgiMinusEffectAt(Vector2 pos, Vector2 target, Action callback)
        {
            PlayEffectAt(pos, target, AgilityMinusPrefab, callback);
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