using UnityEngine;

//DISABLE if using old input system
using UnityEngine.InputSystem;


namespace PhysicsBasedCharacterController
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Camera properties")]
        public GameObject thirdPersonCamera;
        public GameObject firstPersonCamera;
        public Camera mainCamera;
        public CharacterManager characterManager;
        [Space(10)]

        public LayerMask thirdPersonMask;
        public LayerMask firstPersonMask;
        [Space(10)]

        public bool activeThirdPerson = true;
        public bool activeDebug = true;


        /**/


        private void Awake()
        {
            SetCamera();
            SetDebug();
        }


        private void Update()
        {
            //DISABLE if using old input system

            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                activeThirdPerson = !activeThirdPerson;
                SetCamera();
            }

            //DISABLE if using old input system
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                SetDebug();
            }

            //ENABLE if using old input system

            /*

            if (Input.GetKeyDown(KeyCode.M))
            {
                activeThirdPerson = !activeThirdPerson;
                SetCamera();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                SetDebug();
            }

            */
        }


        public void SetCamera()
        {
            if (activeThirdPerson)
            {
                firstPersonCamera.SetActive(false);
                thirdPersonCamera.SetActive(true);

                mainCamera.cullingMask = thirdPersonMask;
            }
            else
            {
                firstPersonCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);

                mainCamera.cullingMask = firstPersonMask;
            }
        }


        public void SetDebug()
        {
            characterManager.debug = !characterManager.debug;
        }
    }
}