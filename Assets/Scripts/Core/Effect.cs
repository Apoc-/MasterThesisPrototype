using UnityEngine;

namespace Core
{
    public class Effect : MonoBehaviour
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;

        public float Alive;
        
        private void Update()
        {
            Alive -= Time.deltaTime;

            if (Alive <= 0)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }

            var pos = new Vector2(transform.position.x, transform.position.y);
            Velocity += Acceleration * Time.deltaTime;;
            pos += Velocity * Time.deltaTime;

            transform.position = pos;
        }
    }
}