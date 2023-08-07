using System;
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
        [SerializeField] private int hitsLimit;
        [SerializeField] private LayerMask layerMaskDamage;
        [SerializeField] private LayerMask layerMaskDestroy;

        private int hitsUntilDestroy;

        public Action DamagerDestroyed;

        private void Start()
        {
            hitsUntilDestroy = hitsLimit;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            int otherLayer = otherCollider2D.gameObject.layer;

            if (IsDamageLayer(otherLayer))
            {
                if (otherCollider2D.TryGetComponent(out IDamageable damageable))
                {
                    DoDamage(damageable);
                    DecreaseHitLimit();
                }
            } 
            else if (IsDestroyLayer(otherLayer))
            {
                DestroySelf();
            }

            bool IsDamageLayer(int otherLayer)
            {
                return (layerMaskDamage & (1 << otherLayer)) != 0 &&
                       hitsUntilDestroy > 0;
            }

            bool IsDestroyLayer(int otherLayer)
            {
                return (layerMaskDestroy & (1 << otherLayer)) != 0;
            }
        }

        private void DoDamage(IDamageable damageable)
        {
            damageable.TakeDamage(damage);
        }

        private void DecreaseHitLimit()
        {
            hitsUntilDestroy--;

            if (hitsUntilDestroy <= 0)
            {
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            DamagerDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}