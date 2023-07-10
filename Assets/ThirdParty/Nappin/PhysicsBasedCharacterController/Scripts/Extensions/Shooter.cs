using UnityEngine;


namespace PhysicsBasedCharacterController
{
    public class Shooter : MonoBehaviour
    {
        [Header("Shooter specs")]
        public GameObject projectile;
        public float speed = 20f;
        public int timer = 2000;

        private float originalTimer;


        /**/



        private void Awake()
        {
            originalTimer = timer;
        }


        private void Update()
        {
            originalTimer--;

            if (originalTimer < 0)
            {
                GameObject instantiatedProjectile = GameObject.Instantiate(projectile, transform.position, transform.rotation);
                instantiatedProjectile.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, speed));
                originalTimer = timer;
            }
        }
    }
}