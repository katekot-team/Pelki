using UnityEngine;
using UnityEngine.Events;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterManager : MonoBehaviour
    {
        public bool DisableMovement = false;
        [Header("Movement specifics")]
        [Tooltip("Layers where the player can stand on")]
        [SerializeField] LayerMask groundMask;
        [Tooltip("Base player speed")]
        public float movementSpeed = 14f;
        [Range(0f, 1f)]
        [Tooltip("Minimum input value to trigger movement")]
        public float crouchSpeedMultiplier = 0.248f;
        [Range(0.01f, 0.99f)]
        [Tooltip("Minimum input value to trigger movement")]
        public float movementThrashold = 0.01f;
        [Space(10)]

        [Tooltip("Speed up multiplier")]
        public float dampSpeedUp = 0.2f;
        [Tooltip("Speed down multiplier")]
        public float dampSpeedDown = 0.1f;


        [Header("Jump and gravity specifics")]
        [Tooltip("Jump velocity")]
        public float jumpVelocity = 20f;
        [Tooltip("Multiplier applied to gravity when the player is falling")]
        public float fallMultiplier = 1.7f;
        [Tooltip("Multiplier applied to gravity when the player is holding jump")]
        public float holdJumpMultiplier = 5f;
        [Range(0f, 1f)]
        [Tooltip("Player friction against floor")]
        public float frictionAgainstFloor = 0.3f;
        [Range(0.01f, 0.99f)]
        [Tooltip("Player friction against wall")]
        public float frictionAgainstWall = 0.839f;
        [Space(10)]

        [Tooltip("Player can long jump")]
        public bool canLongJump = true;


        [Header("Slope and step specifics")]
        [Tooltip("Distance from the player feet used to check if the player is touching the ground")]
        public float groundCheckerThrashold = 0.1f;
        [Tooltip("Distance from the player feet used to check if the player is touching a slope")]
        public float slopeCheckerThrashold = 0.51f;
        [Tooltip("Distance from the player center used to check if the player is touching a step")]
        public float stepCheckerThrashold = 0.6f;
        [Space(10)]

        [Range(1f, 89f)]
        [Tooltip("Max climbable slope angle")]
        public float maxClimbableSlopeAngle = 53.6f;
        [Tooltip("Max climbable step height")]
        public float maxStepHeight = 0.74f;
        [Space(10)]

        [Tooltip("Speed multiplier based on slope angle")]
        public AnimationCurve speedMultiplierOnAngle = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [Range(0.01f, 1f)]
        [Tooltip("Multipler factor on climbable slope")]
        public float canSlideMultiplierCurve = 0.061f;
        [Range(0.01f, 1f)]
        [Tooltip("Multipler factor on non climbable slope")]
        public float cantSlideMultiplierCurve = 0.039f;
        [Range(0.01f, 1f)]
        [Tooltip("Multipler factor on step")]
        public float climbingStairsMultiplierCurve = 0.637f;
        [Space(10)]

        [Tooltip("Multipler factor for gravity")]
        public float gravityMultiplier = 6f;
        [Tooltip("Multipler factor for gravity used on change of normal")]
        public float gravityMultiplyerOnSlideChange = 3f;
        [Tooltip("Multipler factor for gravity used on non climbable slope")]
        public float gravityMultiplierIfUnclimbableSlope = 30f;
        [Space(10)]

        public bool lockOnSlope = false;


        [Header("Wall slide specifics")]
        [Tooltip("Distance from the player head used to check if the player is touching a wall")]
        public float wallCheckerThrashold = 0.8f;
        [Tooltip("Wall checker distance from the player center")]
        public float hightWallCheckerChecker = 0.5f;
        [Space(10)]

        [Tooltip("Multiplier used when the player is jumping from a wall")]
        public float jumpFromWallMultiplier = 30f;
        [Tooltip("Factor used to determine the height of the jump")]
        public float multiplierVerticalLeap = 1f;


        [Header("Sprint and crouch specifics")]
        [Tooltip("Sprint speed")]
        public float sprintSpeed = 20f;
        [Tooltip("Multipler applied to the collider when player is crouching")]
        public float crouchHeightMultiplier = 0.5f;
        [Tooltip("FP camera head height")]
        public Vector3 POV_normalHeadHeight = new Vector3(0f, 0.5f, -0.1f);
        [Tooltip("FP camera head height when crouching")]
        public Vector3 POV_crouchHeadHeight = new Vector3(0f, -0.1f, -0.1f);


        [Header("References")]
        [Tooltip("Character camera")]
        public GameObject characterCamera;
        [Tooltip("Character model")]
        public GameObject characterModel;
        [Tooltip("Character rotation speed when the forward direction is changed")]
        public float characterModelRotationSmooth = 0.1f;
        [Space(10)]

        [Tooltip("Default character mesh")]
        public GameObject meshCharacter;
        [Tooltip("Crouch character mesh")]
        public GameObject meshCharacterCrouch;
        [Tooltip("Head reference")]
        public Transform headPoint;
        [Space(10)]

        [Tooltip("Input reference")]
        public BaseInputReader input;
        [Space(10)]

        public bool debug = true;


        [Header("Events")]
        [SerializeField] UnityEvent OnJump;
        [Space(15)]

        public float minimumVerticalSpeedToLandEvent;
        [SerializeField] UnityEvent OnLand;
        [Space(15)]

        public float minimumHorizontalSpeedToFastEvent;
        [SerializeField] UnityEvent OnFast;
        [Space(15)]

        [SerializeField] UnityEvent OnWallSlide;
        [Space(15)]

        [SerializeField] UnityEvent OnSprint;
        [Space(15)]

        [SerializeField] UnityEvent OnCrouch;
        [Space(15)]



        private Vector3 forward;
        private Vector3 globalForward;
        private Vector3 reactionForward;
        private Vector3 down;
        private Vector3 globalDown;
        private Vector3 reactionGlobalDown;

        private float currentSurfaceAngle;
        private bool currentLockOnSlope;

        private Vector3 wallNormal;
        private Vector3 groundNormal;
        private Vector3 prevGroundNormal;
        private bool prevGrounded;

        private float coyoteJumpMultiplier = 1f;

        private bool isGrounded = false;
        private bool isTouchingSlope = false;
        private bool isTouchingStep = false;
        private bool isTouchingWall = false;
        private bool isJumping = false;
        private bool isCrouch = false;

        private Vector2 axisInput;
        private bool jump;
        private bool jumpHold;
        private bool sprint;
        private bool crouch;

        [HideInInspector]
        public float targetAngle;
        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;
        private float originalColliderHeight;

        private Vector3 currVelocity = Vector3.zero;
        private float turnSmoothVelocity;
        private bool lockRotation = false;


        /**/


        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody>();
            _collider = this.GetComponent<CapsuleCollider>();
            originalColliderHeight = _collider.height;

            SetFriction(frictionAgainstFloor, true);
            currentLockOnSlope = lockOnSlope;
        }


        private void Update()
        {
            //input
            axisInput = input.AxisInput;
            jump = input.Jump;
            jumpHold = input.JumpHold;
            sprint = input.Sprint;
            crouch = input.Crouch;


            if (Input.GetKeyDown(KeyCode.X))
                DisableMovement = !DisableMovement;
        }


        private void FixedUpdate()
        {
            //local vectors
            CheckGrounded();
            CheckStep();
            CheckWall();
            CheckSlopeAndDirections();

            //movement
            if(!DisableMovement)
            {
                MoveCrouch();
                MoveWalk();
                MoveRotation();
                MoveJump();
            }

            //gravity
            ApplyGravity();

            //events
            UpdateEvents();
        }


        #region Checks

        private void CheckGrounded()
        {
            prevGrounded = isGrounded;
            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThrashold, groundMask);
        }


        private void CheckStep()
        {
            bool tmpStep = false;
            Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);

            RaycastHit stepLowerHit;
            if (Physics.Raycast(bottomStepPos, globalForward, out stepLowerHit, stepCheckerThrashold, groundMask))
            {
                RaycastHit stepUpperHit;
                if (RoundValue(stepLowerHit.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), globalForward, out stepUpperHit, stepCheckerThrashold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            RaycastHit stepLowerHit45;
            if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out stepLowerHit45, stepCheckerThrashold, groundMask))
            {
                RaycastHit stepUpperHit45;
                if (RoundValue(stepLowerHit45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(45, Vector3.up) * globalForward, out stepUpperHit45, stepCheckerThrashold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            RaycastHit stepLowerHitMinus45;
            if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(-45, transform.up) * globalForward, out stepLowerHitMinus45, stepCheckerThrashold, groundMask))
            {
                RaycastHit stepUpperHitMinus45;
                if (RoundValue(stepLowerHitMinus45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(-45, Vector3.up) * globalForward, out stepUpperHitMinus45, stepCheckerThrashold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            isTouchingStep = tmpStep;
        }


        private void CheckWall()
        {
            bool tmpWall = false;
            Vector3 tmpWallNormal = Vector3.zero;
            Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + hightWallCheckerChecker, transform.position.z);

            RaycastHit wallHit;
            if (Physics.Raycast(topWallPos, globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(90, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(135, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(180, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(225, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(270, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(315, transform.up) * globalForward, out wallHit, wallCheckerThrashold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }

            isTouchingWall = tmpWall;
            wallNormal = tmpWallNormal;
        }


        private void CheckSlopeAndDirections()
        {
            prevGroundNormal = groundNormal;

            RaycastHit slopeHit;
            if (Physics.SphereCast(transform.position, slopeCheckerThrashold, Vector3.down, out slopeHit, originalColliderHeight / 2f + 0.5f, groundMask))
            {
                groundNormal = slopeHit.normal;

                if (slopeHit.normal.y == 1)
                {

                    forward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    globalForward = forward;
                    reactionForward = forward;

                    SetFriction(frictionAgainstFloor, true);
                    currentLockOnSlope = lockOnSlope;

                    currentSurfaceAngle = 0f;
                    isTouchingSlope = false;

                }
                else
                {
                    //set forward
                    Vector3 tmpGlobalForward = transform.forward.normalized;
                    Vector3 tmpForward = new Vector3(tmpGlobalForward.x, Vector3.ProjectOnPlane(transform.forward.normalized, slopeHit.normal).normalized.y, tmpGlobalForward.z);
                    Vector3 tmpReactionForward = new Vector3(tmpForward.x, tmpGlobalForward.y - tmpForward.y, tmpForward.z);

                    if (currentSurfaceAngle <= maxClimbableSlopeAngle && !isTouchingStep)
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);

                        SetFriction(frictionAgainstFloor, true);
                        currentLockOnSlope = lockOnSlope;
                    }
                    else if (isTouchingStep)
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);

                        SetFriction(frictionAgainstFloor, true);
                        currentLockOnSlope = true;
                    }
                    else
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);

                        SetFriction(0f, true);
                        currentLockOnSlope = lockOnSlope;
                    }

                    currentSurfaceAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    isTouchingSlope = true;
                }

                //set down
                down = Vector3.Project(Vector3.down, slopeHit.normal);
                globalDown = Vector3.down.normalized;
                reactionGlobalDown = Vector3.up.normalized;
            }
            else
            {
                groundNormal = Vector3.zero;

                forward = Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
                globalForward = forward;
                reactionForward = forward;

                //set down
                down = Vector3.down.normalized;
                globalDown = Vector3.down.normalized;
                reactionGlobalDown = Vector3.up.normalized;

                SetFriction(frictionAgainstFloor, true);
                currentLockOnSlope = lockOnSlope;
            }
        }

        #endregion


        #region Move

        private void MoveCrouch()
        {
            if (crouch && isGrounded)
            {
                isCrouch = true;
                if (meshCharacterCrouch != null && meshCharacter != null) meshCharacter.SetActive(false);
                if (meshCharacterCrouch != null) meshCharacterCrouch.SetActive(true);

                float newHeight = originalColliderHeight * crouchHeightMultiplier;
                _collider.height = newHeight;
                _collider.center = new Vector3(0f, -newHeight * crouchHeightMultiplier, 0f);

                headPoint.position = new Vector3(transform.position.x + POV_crouchHeadHeight.x, transform.position.y + POV_crouchHeadHeight.y, transform.position.z + POV_crouchHeadHeight.z);
            }
            else
            {
                isCrouch = false;
                if (meshCharacterCrouch != null && meshCharacter != null) meshCharacter.SetActive(true);
                if (meshCharacterCrouch != null) meshCharacterCrouch.SetActive(false);

                _collider.height = originalColliderHeight;
                _collider.center = Vector3.zero;

                headPoint.position = new Vector3(transform.position.x + POV_normalHeadHeight.x, transform.position.y + POV_normalHeadHeight.y, transform.position.z + POV_normalHeadHeight.z);
            }
        }


        private void MoveWalk()
        {
            float crouchMultiplier = 1f;
            if (isCrouch) crouchMultiplier = crouchSpeedMultiplier;

            if (axisInput.magnitude > movementThrashold)
            {
                targetAngle = Mathf.Atan2(axisInput.x, axisInput.y) * Mathf.Rad2Deg;
                if (characterCamera != null)
                {
                    targetAngle += characterCamera.transform.eulerAngles.y;
                }

                if (!sprint) _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, forward * movementSpeed * crouchMultiplier, ref currVelocity, dampSpeedUp);
                else _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, forward * sprintSpeed * crouchMultiplier, ref currVelocity, dampSpeedUp);
            }
            else _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, Vector3.zero * crouchMultiplier, ref currVelocity, dampSpeedDown);
        }


        private void MoveRotation()
        {
            float angle = Mathf.SmoothDampAngle(characterModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, characterModelRotationSmooth);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            if (!lockRotation) characterModel.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            else
            {
                var lookPos = -wallNormal;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                characterModel.transform.rotation = rotation;
            }
        }


        private void MoveJump()
        {
            
            //jumped from wall
            if (jump && isTouchingWall)
            {
                _rigidbody.velocity += wallNormal * jumpFromWallMultiplier + (Vector3.up * jumpFromWallMultiplier) * multiplierVerticalLeap;
                isJumping = true;

                targetAngle = Mathf.Atan2(wallNormal.x, wallNormal.z) * Mathf.Rad2Deg;

                forward = wallNormal;
                globalForward = forward;
                reactionForward = forward;
            }
            //jumped
            else if (jump && isGrounded && ((isTouchingSlope && currentSurfaceAngle <= maxClimbableSlopeAngle) || !isTouchingSlope) && !isTouchingWall)
            {
                _rigidbody.velocity += Vector3.up * jumpVelocity;
                isJumping = true;
            }

            //is falling
            if (_rigidbody.velocity.y < 0 && !isGrounded) coyoteJumpMultiplier = fallMultiplier;
            else if (_rigidbody.velocity.y > 0.1f && (currentSurfaceAngle <= maxClimbableSlopeAngle || isTouchingStep))
            {
                //is short jumping
                if (!jumpHold || !canLongJump) coyoteJumpMultiplier = 1f;
                //is long jumping
                else coyoteJumpMultiplier = 1f / holdJumpMultiplier;
            }
            else
            {
                isJumping = false;
                coyoteJumpMultiplier = 1f;
            }
        }

        #endregion


        #region Gravity

        private void ApplyGravity()
        {
            Vector3 gravity = Vector3.zero;

            if (currentLockOnSlope || isTouchingStep) gravity = down * gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier;
            else gravity = globalDown * gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier;

            //avoid little jump
            if (groundNormal.y != 1 && groundNormal.y != 0 && isTouchingSlope && prevGroundNormal != groundNormal)
            {
                //Debug.Log("Added correction jump on slope");
                gravity *= gravityMultiplyerOnSlideChange;
            }

            //slide if angle too big
            if (groundNormal.y != 1 && groundNormal.y != 0 && (currentSurfaceAngle > maxClimbableSlopeAngle && !isTouchingStep))
            {
                //Debug.Log("Slope angle too high, character is sliding");
                if (currentSurfaceAngle > 0f && currentSurfaceAngle <= 30f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope * -Physics.gravity.y;
                else if (currentSurfaceAngle > 30f && currentSurfaceAngle <= 89f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
            }

            //friction when touching wall
            if (isTouchingWall && _rigidbody.velocity.y < 0) gravity *= frictionAgainstWall;

            _rigidbody.AddForce(gravity);
        }

        #endregion


        #region Events

        private void UpdateEvents()
        {
            if ((jump && isGrounded && ((isTouchingSlope && currentSurfaceAngle <= maxClimbableSlopeAngle) || !isTouchingSlope)) || (jump && !isGrounded && isTouchingWall)) OnJump.Invoke();
            if (isGrounded && !prevGrounded && _rigidbody.velocity.y > -minimumVerticalSpeedToLandEvent) OnLand.Invoke();
            if (Mathf.Abs(_rigidbody.velocity.x) + Mathf.Abs(_rigidbody.velocity.z) > minimumHorizontalSpeedToFastEvent) OnFast.Invoke();
            if (isTouchingWall && _rigidbody.velocity.y < 0) OnWallSlide.Invoke();
            if (sprint) OnSprint.Invoke();
            if (isCrouch) OnCrouch.Invoke();
        }

        #endregion


        #region Friction and Round

        private void SetFriction(float _frictionWall, bool _isMinimum)
        {
            _collider.material.dynamicFriction = 0.6f * _frictionWall;
            _collider.material.staticFriction = 0.6f * _frictionWall;

            if (_isMinimum) _collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            else _collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
        }


        private float RoundValue(float _value)
        {
            float unit = (float)Mathf.Round(_value);

            if (_value - unit < 0.000001f && _value - unit > -0.000001f) return unit;
            else return _value;
        }

        #endregion


        #region GettersSetters

        public bool GetGrounded() { return isGrounded; }
        public bool GetTouchingSlope() { return isTouchingSlope; }
        public bool GetTouchingStep() { return isTouchingStep; }
        public bool GetTouchingWall() { return isTouchingWall; }
        public bool GetJumping() { return isJumping; }
        public bool GetCrouching() { return isCrouch; }
        public float GetOriginalColliderHeight() { return originalColliderHeight; }

        public void SetLockRotation(bool _lock) { lockRotation = _lock; }

        #endregion


        #region Gizmos

        private void OnDrawGizmos()
        {
            if (debug)
            {
                _rigidbody = this.GetComponent<Rigidbody>();
                _collider = this.GetComponent<CapsuleCollider>();

                Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);
                Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + hightWallCheckerChecker, transform.position.z);

                //ground and slope
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThrashold);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), slopeCheckerThrashold);

                //direction
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + forward * 2f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + globalForward * 2);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + reactionForward * 2f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + down * 2f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, transform.position + globalDown * 2f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, transform.position + reactionGlobalDown * 2f);

                //step check
                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + globalForward * stepCheckerThrashold);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + new Vector3(0f, maxStepHeight, 0f) + globalForward * (stepCheckerThrashold + 0.05f));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * stepCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(45, Vector3.up) * (globalForward * stepCheckerThrashold) + new Vector3(0f, maxStepHeight, 0f));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(-45, transform.up) * (globalForward * stepCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(-45, Vector3.up) * (globalForward * stepCheckerThrashold) + new Vector3(0f, maxStepHeight, 0f));

                //wall check
                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + globalForward * wallCheckerThrashold);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * wallCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(90, transform.up) * (globalForward * wallCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(135, transform.up) * (globalForward * wallCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(180, transform.up) * (globalForward * wallCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(225, transform.up) * (globalForward * wallCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(270, transform.up) * (globalForward * wallCheckerThrashold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(315, transform.up) * (globalForward * wallCheckerThrashold));
            }
        }

        #endregion
    }
}