using Pelki.Frameworks.Spine;
using Pelki.Gameplay.Characters.Movements;
using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : Entity
    {
        private enum CharacterState
        {
            Idle = 0,
            Run = 1,
            Rise = 2,
            Fall = 3,
            Attack = 4,
            RangedAttack = 5,
        }

        [SerializeField] private GroundMover mover;
        [SerializeField] private ProjectileSpawner projectileSpawner;
        [SerializeField] private float attackCooldown;
        [SerializeField] private SpineSkeletonAnimator skeletonAnimator;

        private const string IdleHash = "idle";
        private const string RunHash = "run";
        private const string RiseHash = "rise";
        private const string FallHash = "fall";
        private const string AttackHash = "attack";
        private const string RangedAttackHash = "rangedAttack";

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
            skeletonAnimator.Initialize();
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

                skeletonAnimator.PlayOneShot(RangedAttackHash, lockStateChanging: true);
            }

            skeletonAnimator.SetFlip(input.Horizontal);

            bool isStateChanged = previousState != currentState;
            previousState = currentState;

            if (isStateChanged)
                HandleStateChanged();
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

        //klavikus: require change according to approved states list
        private void HandleStateChanged()
        {
            string stateName;

            //klavikus: Idea -> change to dictionary<State, Hash>
            switch (currentState)
            {
                case CharacterState.Idle:
                    stateName = IdleHash;
                    break;
                case CharacterState.Run:
                    stateName = RunHash;
                    break;
                case CharacterState.Rise:
                    stateName = RiseHash;
                    break;
                case CharacterState.Fall:
                    stateName = FallHash;
                    break;
                case CharacterState.Attack:
                    stateName = AttackHash;
                    break;
                case CharacterState.RangedAttack:
                    stateName = RangedAttackHash;
                    break;
                default:
                    return;
            }

            skeletonAnimator.PlayAnimationForState(stateName, 0);
        }
    }
}