using Pelki.Gameplay.DamageSystem;
using UnityEngine;

namespace Pelki.Gameplay.Enemies
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Damager damager;
        [SerializeField] private Rigidbody2D rigidbody2D;
        [Min(0f)] 
        [SerializeField] private float force;

        public void Construct(Vector2 direction)
        {
            StartMovement(direction);
        }

        private void OnEnable()
        {
            damager.DamagerDestroyed += DestroySelf;
        }

        private void OnDisable()
        {
            damager.DamagerDestroyed -= DestroySelf;
        }

        public void StartMovement(Vector2 direction)
        {
            rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}