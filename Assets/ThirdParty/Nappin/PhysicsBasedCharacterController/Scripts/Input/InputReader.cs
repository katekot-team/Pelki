using UnityEngine;
using UnityEngine.Events;

//DISABLE if using old input system
using UnityEngine.InputSystem;


namespace PhysicsBasedCharacterController
{
    public class InputReader : BaseInputReader
    {
        [Header("Input specs")]
        public UnityEvent changedInputToMouseAndKeyboard;

        public UnityEvent changedInputToGamepad;

        [Header("Enable inputs")]
        public bool enableJump = true;

        public bool enableCrouch = true;
        public bool enableSprint = true;

        public override Vector2 AxisInput { get; protected set; }
        public override Vector2 CameraInput { get; protected set; } = Vector2.zero;
        public override bool Jump { get; protected set; }
        public override bool JumpHold { get; protected set; }
        public override float Zoom { get; protected set; }
        public override bool Sprint { get; protected set; }
        public override bool Crouch { get; protected set; }

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
            AxisInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }


        public void OnJump()
        {
            if (enableJump)
            {
                Jump = true;
                JumpHold = true;

                hasJumped = true;
                skippedFrame = false;
            }
        }


        public void JumpEnded()
        {
            Jump = false;
            JumpHold = false;
        }


        private void FixedUpdate()
        {
            if (hasJumped && skippedFrame)
            {
                Jump = false;
                hasJumped = false;
            }

            if (!skippedFrame && enableJump) skippedFrame = true;
        }


        //DISABLE if using old input system
        public void OnCamera(InputAction.CallbackContext ctx)
        {
            Vector2 pointerDelta = ctx.ReadValue<Vector2>();
            Vector2 newCameraInput = CameraInput;
            newCameraInput.x += pointerDelta.x;
            newCameraInput.y += pointerDelta.y;
            CameraInput = newCameraInput;
            GetDeviceNew(ctx);
        }


        //DISABLE if using old input system
        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (enableSprint) Sprint = true;
        }


        //DISABLE if using old input system
        public void SprintEnded(InputAction.CallbackContext ctx)
        {
            Sprint = false;
        }


        //DISABLE if using old input system
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (enableCrouch) Crouch = true;
        }


        //DISABLE if using old input system
        public void CrouchEnded(InputAction.CallbackContext ctx)
        {
            Crouch = false;
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