using NaughtyAttributes;
using Pelki.Interfaces;
using UnityEngine;

namespace Pelki.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        [MinValue(0)]
        [SerializeField] private int damage;
        [SerializeField] private LayerMask layerMask;

        private void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            var otherLayer = otherCollider2D.gameObject.layer;
            
            if ( (layerMask & (1 << otherLayer)) != 0 && otherCollider2D.TryGetComponent(out IDamageable damageable))
            {
                DoDamage(damageable);
            }
        }

        private void DoDamage(IDamageable damageable)
        {
            damageable.TakeDamage(damage);
        }
    }
}
