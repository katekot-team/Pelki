using UnityEngine;
using UnityEngine.AI;

namespace PhysicsBasedCharacterController
{
    public class AnimatedController : MonoBehaviour
    {
        [Header("References")]
        public CharacterManager characterManager;
        public Rigidbody rigidbodyCharacter;
        public NavMeshAgent navAgentCharacter;
        [SerializeField] LayerMask groundMask;
        [Space(10)]

        [Header("Animation specifics")]
        public float velocityAnimationMultiplier = 1f;
        public bool lockRotationOnWall = true;
        public float groundCheckerThrashold = 0.4f;
        public float climbThreshold = 0.5f;


        private Animator anim;
        private float originalColliderHeight;


        /**/


        private void Awake()
        {
            anim = this.GetComponent<Animator>();
        }


        private void Start()
        {
            originalColliderHeight = characterManager.GetOriginalColliderHeight();
        }


        private void Update()
        {
            if(!rigidbodyCharacter.isKinematic)
                anim.SetFloat("velocity", rigidbodyCharacter.velocity.magnitude * velocityAnimationMultiplier);
            
            if(navAgentCharacter)
                if(navAgentCharacter.enabled)
                    anim.SetFloat("velocity", navAgentCharacter.velocity.magnitude * velocityAnimationMultiplier);

            anim.SetBool("isGrounded", CheckAnimationGrounded());

            anim.SetBool("isJump", characterManager.GetJumping());

            anim.SetBool("isTouchWall", characterManager.GetTouchingWall());
            if (lockRotationOnWall) characterManager.SetLockRotation(characterManager.GetTouchingWall());

            anim.SetBool("isClimb", characterManager.GetTouchingWall() && rigidbodyCharacter.velocity.y > climbThreshold);

            anim.SetBool("isCrouch", characterManager.GetCrouching());
        }


        private bool CheckAnimationGrounded()
        {
            return Physics.CheckSphere(characterManager.transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThrashold, groundMask);
        }
    }
}