using System;
using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters.Movements
{
    public class GroundMover : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float movementThrashold;
        [SerializeField] private float movementSpeed;
        [SerializeField] private AnimationCurve speedAccelerationCurve;
        [SerializeField] private float accelerationTime;
        [SerializeField] private AnimationCurve speedBreakingCurve;
        [SerializeField] private float brakingTime;

        [Header("Jump and gravity specifics")]
        [SerializeField] private float jumpVelocity = 12f;

        [SerializeField] private float gravityFactor;

        [Header("Other")]
        [SerializeField] private LayerMask groundMask;

        [SerializeField] private float radiusGroundCheck;
        [SerializeField] private Rigidbody2D rigidBody;


        [Header("Debug")]
        [SerializeField] private bool isDrawDebug;

        [SerializeField] private Color checkGroundDebugRayColor;

        //for Andrey: обрати внимание что сделал буффер для оптимизации памяти, что бы не выделять постоянно при рейкасте
        private readonly Collider2D[] raycastBufferGroundCheckColliders = new Collider2D[1];
        private readonly RaycastHit2D[] raycastBufferRaycastHit2D = new RaycastHit2D[1];

        private IInput input;

        private bool isGrounded;
        private bool isJumping;

        private float elapsedTime;
        private float brakingElapsedTime;

        private bool IsInputJump => input.IsJump;

        public void Construct(IInput input)
        {
            this.input = input;
        }

        //sttrox: точно ли Fixed Update?
        private void FixedUpdate()
        {
            isGrounded = CheckGrounded();

            DoMove();
            TryDoJump();

            ApplyGravity();
        }

        private bool CheckGrounded()
        {
            //todo sttrox: вынести кэш (Vector2)transform.position в начало кадра
            Vector2 colliderGroundPoint = transform.position;

            int countGroundCollisions = Physics2D.OverlapCircleNonAlloc(colliderGroundPoint, radiusGroundCheck,
                raycastBufferGroundCheckColliders, groundMask);

            if (isDrawDebug)
            {
                Debug.DrawRay(colliderGroundPoint, Vector2.down * radiusGroundCheck, checkGroundDebugRayColor);
            }

            bool result = countGroundCollisions > 0;
            return result;
        }

        private void ApplyGravity()
        {
            Vector3 gravity = Vector3.zero;

            gravity = Vector3.down * gravityFactor * -Physics.gravity.y;

            rigidBody.AddForce(gravity);
        }

        private void DoMove()
        {
            float rawHorizontalInput = input.RawHorizontal;
            float absRawHorizontalInput = Mathf.Abs(rawHorizontalInput);
            Vector2 currentVelocity = rigidBody.velocity;

            if (absRawHorizontalInput < movementThrashold)
            {
                brakingElapsedTime += Time.deltaTime;
                var percentBrakingTime = 1 - (brakingElapsedTime / brakingTime);

                float brakingClamping = speedBreakingCurve.Evaluate(percentBrakingTime);
                Vector2 newVelocity = new Vector2(currentVelocity.x * brakingClamping, currentVelocity.y);
                rigidBody.velocity = newVelocity;

                elapsedTime = 0f;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                var percentTime = elapsedTime / accelerationTime;

                float speedClamping = speedAccelerationCurve.Evaluate(percentTime);
                var speedFactor = Mathf.Min(speedClamping, absRawHorizontalInput);
                speedFactor *= Math.Sign(rawHorizontalInput);

                Vector2 newVelocity = new Vector2(speedFactor * movementSpeed, currentVelocity.y);
                rigidBody.velocity = newVelocity;

                brakingElapsedTime = 0f;
            }
        }

        private bool TryDoJump()
        {
            if (IsInputJump && isGrounded)
            {
                rigidBody.velocity += Vector2.up * jumpVelocity;
                isJumping = true;
            }

            if (IsFalling())
            {
                isJumping = false;
            }

            return isJumping;

            bool IsFalling()
            {
                return rigidBody.velocity.y < 0;
            }
        }
    }
}