using UnityEngine;
using UnityEngine.Events;

//DISABLE if using old input system
using UnityEngine.InputSystem;


namespace PhysicsBasedCharacterController
{
    public class InputReader : MonoBehaviour
    {
        [Header("Input specs")]
        public UnityEvent changedInputToMouseAndKeyboard;
        public UnityEvent changedInputToGamepad;

        [Header("Enable inputs")]
        public bool enableJump = true;
        public bool enableCrouch = true;
        public bool enableSprint = true;


        [HideInInspector]
        public Vector2 axisInput;
        [HideInInspector]
        public Vector2 cameraInput = Vector2.zero;
        [HideInInspector]
        public bool jump;
        [HideInInspector]
        public bool jumpHold;
        [HideInInspector]
        public float zoom;
        [HideInInspector]
        public bool sprint;
        [HideInInspector]
        public bool crouch;


        private bool hasJumped = false;
        private bool skippedFrame = false;
        private bool isMouseAndKeyboard = true;
        private bool oldInput = true;

        //DISABLE if using old input system
        private MovementActions movementActions;


        /**/


        //DISABLE if using old input system
        private void Awake()
        {
            movementActions = new MovementActions();

            movementActions.Gameplay.Movement.performed += ctx => OnMove(ctx);

            movementActions.Gameplay.Jump.performed += ctx => OnJump();
            movementActions.Gameplay.Jump.canceled += ctx => JumpEnded();

            movementActions.Gameplay.Camera.performed += ctx => OnCamera(ctx);

            movementActions.Gameplay.Sprint.performed += ctx => OnSprint(ctx);
            movementActions.Gameplay.Sprint.canceled += ctx => SprintEnded(ctx);

            movementActions.Gameplay.Crouch.performed += ctx => OnCrouch(ctx);
            movementActions.Gameplay.Crouch.canceled += ctx => CrouchEnded(ctx);
        }


        //ENABLE if using old input system
        private void Update()
        {
            /*
             
            axisInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

            if (enableJump)
            {
                if (Input.GetButtonDown("Jump")) OnJump();
                if (Input.GetButtonUp("Jump")) JumpEnded();
            }

            if (enableSprint) sprint = Input.GetButton("Fire3");
            if (enableCrouch) crouch = Input.GetButton("Fire1");

            GetDeviceOld();

            */
        }


        //DISABLE if using old input system
        private void GetDeviceNew(InputAction.CallbackContext ctx)
        {
            oldInput = isMouseAndKeyboard;

            if (ctx.control.device is Keyboard || ctx.control.device is Mouse) isMouseAndKeyboard = true;
            else isMouseAndKeyboard = false;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();
        }


        //ENABLE if using old input system
        private void GetDeviceOld()
        {
            /*

            oldInput = isMouseAndKeyboard;

            if (Input.GetJoystickNames().Length > 0) isMouseAndKeyboard = false;
            else isMouseAndKeyboard = true;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();

            */
        }


        #region Actions

        //DISABLE if using old input system
        public void OnMove(InputAction.CallbackContext ctx)
        {
            axisInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }


        public void OnJump()
        {
            if (enableJump)
            {
                jump = true;
                jumpHold = true;

                hasJumped = true;
                skippedFrame = false;
            }
        }


        public void JumpEnded()
        {
            jump = false;
            jumpHold = false;
        }



        private void FixedUpdate()
        {
            if (hasJumped && skippedFrame)
            {
                jump = false;
                hasJumped = false;
            }
            if (!skippedFrame && enableJump) skippedFrame = true;
        }



        //DISABLE if using old input system
        public void OnCamera(InputAction.CallbackContext ctx)
        {
            Vector2 pointerDelta = ctx.ReadValue<Vector2>();
            cameraInput.x += pointerDelta.x;
            cameraInput.y += pointerDelta.y;
            GetDeviceNew(ctx);
        }


        //DISABLE if using old input system
        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (enableSprint) sprint = true;
        }


        //DISABLE if using old input system
        public void SprintEnded(InputAction.CallbackContext ctx)
        {
            sprint = false;
        }


        //DISABLE if using old input system
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (enableCrouch) crouch = true;
        }


        //DISABLE if using old input system
        public void CrouchEnded(InputAction.CallbackContext ctx)
        {
            crouch = false;
        }

        #endregion


        #region Enable / Disable

        //DISABLE if using old input system
        private void OnEnable()
        {
            movementActions.Enable();
        }


        //DISABLE if using old input system
        private void OnDisable()
        {
            movementActions.Disable();
        }

        #endregion
    }
}