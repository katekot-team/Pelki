using Pelki.Gameplay.Enemies;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private Projectile projectilePrefab;

        private Vector2 direction;

        private void ShootProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab, 
                projectileSpawnPoint.position, 
                projectileSpawnPoint.rotation
            );

            projectile.Construct(direction);
        }
    }
}