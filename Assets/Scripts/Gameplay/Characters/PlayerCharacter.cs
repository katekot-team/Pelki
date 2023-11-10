using Pelki.Gameplay.Characters.Animations;
using Pelki.Gameplay.Characters.Movements;
using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private GroundMover mover;
        [SerializeField] private ProjectileSpawner projectileSpawner;
        [SerializeField] private float attackCooldown;
        [SerializeField] private PlayerAnimator playerAnimator;
        

        private float reloadCompletionTime;
        private bool canPerformRangedAttack;
        private IInput input;
        private CharacterState currentState;
        private CharacterState previousState;

        public void Construct(IInput input)
        {
            this.input = input;
            canPerformRangedAttack = true;

            mover.Construct(input);
            playerAnimator.Initialize();
        }

        //klavikus: Too many conditions - looks like reason for FSM
        private void Update()
        {
            if (!canPerformRangedAttack && Time.time > reloadCompletionTime)
            {
                canPerformRangedAttack = true;
            }

            if (mover.IsGrounded)
            {
                //move,idle

                currentState = mover.IsIdle ? CharacterState.Idle : CharacterState.Run;
            }

            if (!mover.IsGrounded && mover.IsJumping)
            {
                //start jump

                currentState = CharacterState.Rise;
            }

            if (!mover.IsGrounded && !mover.IsJumping)
            {
                //falling

                currentState = CharacterState.Fall;
            }

            if (IsPerformingRangedAttack())
            {
                RangedAttack();
                playerAnimator.PlayRangedAttack();
            }

            bool isStateChanged = previousState != currentState;
            previousState = currentState;

            if (isStateChanged) 
                playerAnimator.UpdateState(currentState);

            playerAnimator.SetFlip(input.Horizontal);
        }

        private bool IsPerformingRangedAttack()
        {
            bool isCalled = input.IsRangedAttacking;
            return isCalled && canPerformRangedAttack;
        }

        private void RangedAttack()
        {
            projectileSpawner.Shoot(transform.right);
            canPerformRangedAttack = false;
            reloadCompletionTime = Time.time + attackCooldown;
        }
    }
}