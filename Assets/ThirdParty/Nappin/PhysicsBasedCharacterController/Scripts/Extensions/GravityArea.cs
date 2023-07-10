using System.Collections.Generic;
using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(Collider))]
    public class GravityArea : MonoBehaviour
    {
        [Header("Area properties")]
        public Vector3 gravityForce = new Vector3(0f, 1.37f, 0f);

        private List<Rigidbody> rigidbodies = new List<Rigidbody>();


        /**/


        private void FixedUpdate()
        {
            if (rigidbodies.Count > 0)
            {
                for (int i = 0; i < rigidbodies.Count; i++)
                {
                    Rigidbody rb = rigidbodies[i];
                    rb.velocity = new Vector3(rb.velocity.x * gravityForce.x, gravityForce.y, rb.velocity.z * gravityForce.z);
                }
            }
        }


        #region Collision detection

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null && !rigidbodies.Contains(rigidbody)) rigidbodies.Add(rigidbody);
        }


        private void OnTriggerExit(Collider other)
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null && rigidbodies.Contains(rigidbody)) rigidbodies.Remove(rigidbody);
        }

        #endregion
    }
}