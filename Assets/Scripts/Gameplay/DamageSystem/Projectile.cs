using UnityEngine;

namespace Pelki.Gameplay.DamageSystem
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Damager damager;
        [SerializeField] private Rigidbody2D rigidbody2d;
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

        private void Start()
        {
            rigidbody2d.velocity = direction * speed;
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