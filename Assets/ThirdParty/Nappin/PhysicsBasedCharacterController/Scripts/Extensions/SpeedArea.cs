using System.Collections.Generic;
using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(Collider))]
    public class SpeedArea : MonoBehaviour
    {
        [Header("Area properties")]
        public float velocityMultiplier = 1.1f;


        private List<Rigidbody> rigidbodies = new List<Rigidbody>();


        /**/


        private void FixedUpdate()
        {
            if (rigidbodies.Count > 0)
            {
                for (int i = 0; i < rigidbodies.Count; i++)
                {
                    Rigidbody rb = rigidbodies[i];
                    rb.velocity *= velocityMultiplier;
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