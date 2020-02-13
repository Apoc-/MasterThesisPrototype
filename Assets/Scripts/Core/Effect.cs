using System;
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
        
        private void Update()
        {
            MaxLifeTime -= Time.deltaTime;

            if (ReachedTarget() || MaxLifeTime <= 0)
            {
                TargetReachedCallback?.Invoke();
                KillEffect();
                return;
            }

            var pos = new Vector2(transform.position.x, transform.position.y);
            Velocity += Acceleration * Time.deltaTime;;
            pos += Velocity * Time.deltaTime;

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