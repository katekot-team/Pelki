using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(Collider))]
    public class PlatformSensor : MonoBehaviour
    {
        private MovingPlatform movingPlatform;
        private BoxCollider boxCollider;


        /**/


        private void Awake()
        {
            movingPlatform = this.transform.parent.GetComponent<MovingPlatform>();
            if (!movingPlatform)
                Debug.LogError(transform.parent.name + "No moving platform assigned!");
        }


        #region Collision detection

        private void OnTriggerEnter(Collider other)
        {
            
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null && rigidbody != movingPlatform.GetComponent<Rigidbody>())
            {
                movingPlatform.Add(rigidbody);
                return;
            }
            //if(other.attachedRigidbody != null && other.attachedRigidbody != movingPlatform.GetComponent<Rigidbody>()) movingPlatform.Add(rigidbody);
            if (other.attachedRigidbody)
            {
                movingPlatform.Add(other.attachedRigidbody);
            }
                
        }


        private void OnTriggerExit(Collider other)
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null && rigidbody != movingPlatform.GetComponent<Rigidbody>())
            {
                movingPlatform.Remove(rigidbody);
                return;
            }
            if (other.attachedRigidbody)
            {
                movingPlatform.Remove(other.attachedRigidbody);
                return;
            }
        }

        #endregion
    }
}