using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(BoxCollider))]
    public class TrampolineSensor : MonoBehaviour
    {
        private TrampolinePlatform trampolinePlatform;


        /**/


        private void Awake()
        {
            trampolinePlatform = this.transform.parent.GetComponent<TrampolinePlatform>();
        }


        #region Collision detection

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null && rigidbody != trampolinePlatform.GetComponent<Rigidbody>()) trampolinePlatform.Add(rigidbody, rigidbody.velocity.y);
        }


        private void OnTriggerExit(Collider other)
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null && rigidbody != trampolinePlatform.GetComponent<Rigidbody>()) trampolinePlatform.Remove(rigidbody);
        }

        #endregion
    }
}