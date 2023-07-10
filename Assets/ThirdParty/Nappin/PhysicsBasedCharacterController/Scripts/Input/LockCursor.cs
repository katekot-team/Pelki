using UnityEngine;


namespace PhysicsBasedCharacterController
{
    public class LockCursor : MonoBehaviour
    {
        public bool lockCursor = false;


        /**/


        private void Awake()
        {
            if(lockCursor) Cursor.lockState = CursorLockMode.Locked;
        }
    }
}