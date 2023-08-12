using Pelki.Gameplay.DamageSystem;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;

        public void Shoot(Vector2 direction)
        {
            Projectile projectile = Instantiate(projectilePrefab, 
                transform.position, 
                transform.rotation
            );

            projectile.Initialize(direction);
        }
    }
}