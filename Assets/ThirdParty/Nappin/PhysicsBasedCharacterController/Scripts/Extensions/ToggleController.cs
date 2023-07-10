using UnityEngine;


namespace PhysicsBasedCharacterController
{
    public class ToggleController : MonoBehaviour
    {
        [Header("Camera specs")]
        public GameObject gamepadCamera;
        public GameObject mouseAndKeyboardCamera;


        /**/


        public void isInputGamepad(bool _status)
        {
            gamepadCamera.SetActive(_status);
            mouseAndKeyboardCamera.SetActive(!_status);
        }
    }
}