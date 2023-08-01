using NaughtyAttributes;
using Pelki.Interfaces;
using UnityEngine;

namespace Pelki.Enemies
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Damager : MonoBehaviour
    {
        [SerializeField, MinValue(0)] private int damage;
        
        private void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            if (otherCollider2D.TryGetComponent(out IDamageable damageable))
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
