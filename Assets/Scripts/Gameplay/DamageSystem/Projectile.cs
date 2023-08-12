using UnityEngine;

namespace Pelki.Gameplay.DamageSystem
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Damager damager;
        [Min(0f)]
        [SerializeField] private float speed;

        private Vector2 direction;

        public void Initialize(Vector2 attackDirection)
        {
            direction = attackDirection;
        }

        private void OnEnable()
        {
            damager.DamagerDestroyed += DestroySelf;
        }

        private void FixedUpdate()
        {
            Vector3 unitVector3 = new Vector3(direction.x, direction.y, 0.0f);
            transform.Translate(unitVector3 * speed * Time.deltaTime);
        }

        private void OnDisable()
        {
            damager.DamagerDestroyed -= DestroySelf;
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}