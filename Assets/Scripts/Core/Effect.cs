using System;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class Effect : MonoBehaviour
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;

        public Action TargetReachedCallback;

        public Vector2 Target;

        public float MaxLifeTime = 3f;

        public bool UseUnscaledTime = true;
        
        private float DeltaTime => UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        
        private void Update()
        {
            MaxLifeTime -= DeltaTime;

            if (ReachedTarget() || MaxLifeTime <= 0)
            {
                TargetReachedCallback?.Invoke();
                KillEffect();
                return;
            }

            var pos = new Vector2(transform.position.x, transform.position.y);
            Velocity += Acceleration * DeltaTime;
            pos += Velocity * DeltaTime;

            transform.position = pos;
        }

        

        private void KillEffect()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
        private bool ReachedTarget()
        {
            var dist = Vector2.Distance(transform.position, Target);
            return dist < 100f;
        }
    }
}