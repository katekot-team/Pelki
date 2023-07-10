using System.Collections.Generic;
using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(Collider))]
    public class Ladder : MonoBehaviour
    {
        [Header("Climb properties")]
        public float climbSpeed = 7f;
        public float forceOnDismount = -200f;
        public InputReader input;

        private List<Rigidbody> rigidbodies = new List<Rigidbody>();


        /**/


        private void FixedUpdate()
        {
            if (rigidbodies.Count > 0)
            {
                for (int i = 0; i < rigidbodies.Count; i++)
                {
                    Rigidbody rb = rigidbodies[i];

                    if (input.axisInput.y > 0) rb.velocity = new Vector3(0f, climbSpeed, 0f);
                    else if (input.axisInput.y < 0) rb.AddForce(forceOnDismount * this.transform.forward);
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