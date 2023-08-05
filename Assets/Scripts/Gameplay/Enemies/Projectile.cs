using Pelki.Gameplay.DamageSystem;
using UnityEngine;

namespace Pelki.Gameplay.Enemies
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Damager damager;

        private void Start()
        {
            damager.DamagerDestroyed += DestroySelf;
        }
        
        private void DestroySelf() => Destroy(gameObject);
    }
}