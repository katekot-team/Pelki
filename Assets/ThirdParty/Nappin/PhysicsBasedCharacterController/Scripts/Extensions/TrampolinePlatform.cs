using System.Collections.Generic;
using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TrampolinePlatform : MonoBehaviour
    {
        [Header("Trampoline properties")]
        public float bounceStrength = 20f;


        private List<Rigidbody> rigidbodies = new List<Rigidbody>();
        private List<float> velocities = new List<float>();


        /**/


        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody rb = collision.transform.GetComponent<Rigidbody>();
            if (rigidbodies.Contains(rb))
            {
                //Debug.Log(velocities[rigidbodies.IndexOf(rb)]);
                rb.AddForce(bounceStrength * transform.up * -velocities[rigidbodies.IndexOf(rb)], ForceMode.Impulse);
            }
        }


        #region Handle list

        public void Add(Rigidbody _rb, float _velocity_y)
        {
            if (!rigidbodies.Contains(_rb))
            {
                rigidbodies.Add(_rb);
                velocities.Add(_velocity_y);
            }
        }


        public void Remove(Rigidbody _rb)
        {
            if (rigidbodies.Contains(_rb))
            {
                rigidbodies.Remove(_rb);
                velocities.Remove(rigidbodies.IndexOf(_rb));
            }
        }

        #endregion
    }
}