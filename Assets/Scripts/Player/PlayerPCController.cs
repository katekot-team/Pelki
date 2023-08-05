using UnityEngine;

public class PlayerPCController : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    //SichTM: My temporal addition for this class for testing that projectile is working correctly.
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Rigidbody2D projectilePrefab;
    [Min(0f)] 
    [SerializeField] private float force;
    
    private void Update()
    {
        playerManager.move = Input.GetAxis("Horizontal");
        playerManager.jump = Input.GetButtonDown("Jump");
        playerManager.dash = Input.GetButtonDown("Dash");
        playerManager.hit = Input.GetButtonDown("Fire1");
        playerManager.fire = Input.GetButtonDown("Fire2");

        //SichTM: My temporal addition for this class for testing that projectile is working correctly.
        if (Input.GetButtonDown("Fire1"))
        {
            ShootProjectile();
        }
    }
    //SichTM: My temporal addition for this class for testing that projectile is working correctly.
    private void ShootProjectile()
    {
        Rigidbody2D projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, 
            projectileSpawnPoint.rotation);

        Vector2 direction = projectileSpawnPoint.right * force;
        projectile.AddForce(direction, ForceMode2D.Impulse);
    }
}
