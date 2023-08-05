using NaughtyAttributes;
using Pelki.Interfaces;
using UnityEngine;

namespace Pelki.Gameplay.DamageSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        [MinValue(0)] 
        [SerializeField] private int damage;
        [MinValue(0)] 
        [SerializeField] private int hitLimit;
        [SerializeField] private LayerMask layerMaskDamage;
        [SerializeField] private LayerMask layerMaskDestroy;

        public event Destroyed DamagerDestroyed;
        
        public delegate void Destroyed();
        
        private void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            int otherLayer = otherCollider2D.gameObject.layer;

            if ((layerMaskDamage & (1 << otherLayer)) != 0 && hitLimit > 0)
            {
                if (otherCollider2D.TryGetComponent(out IDamageable damageable))
                {
                    DoDamage(damageable);
                    DecreaseHitLimit();
                }
            } 
            else if ((layerMaskDestroy & (1 << otherLayer)) != 0)
            {
                DestroySelf();
            }
        }

        private void DoDamage(IDamageable damageable)
        {
            damageable.TakeDamage(damage);
        }

        private void DecreaseHitLimit()
        {
            hitLimit--;

            if (hitLimit <= 0)
            {
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            DamagerDestroyed();
            Destroy(gameObject);
        }
    }
}