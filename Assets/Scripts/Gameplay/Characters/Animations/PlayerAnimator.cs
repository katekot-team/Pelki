using Pelki.Frameworks.Spine;
using UnityEngine;

namespace Pelki.Gameplay.Characters.Animations
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SpineSkeletonAnimator skeletonAnimator;

        private const string IdleHash = "idle";
        private const string RunHash = "run";
        private const string RiseHash = "rise";
        private const string FallHash = "fall";
        private const string AttackHash = "attack";
        private const string RangedAttackHash = "rangedAttack";

        private const int AttackTrackIndex = 2;

        private CharacterState previousState;
        private CharacterState currentState;

        public void Initialize()
        {
            skeletonAnimator.Initialize();
        }

        public void SetFlip(float inputHorizontal)
        {
            skeletonAnimator.SetFlip(inputHorizontal);
        }

        public void SetState(CharacterState characterState)
        {
            currentState = characterState;

            if (previousState == currentState)
            {
                return;
            }

            previousState = currentState;

            PlayStateAnimation(characterState);
        }

        public void PlayMeleeAttack()
        {
            skeletonAnimator.PlayOneShot(AttackHash, AttackTrackIndex);
        }

        public void PlayRangedAttack()
        {
            skeletonAnimator.PlayOneShot(RangedAttackHash, AttackTrackIndex);
        }

        //klavikus: require change according to approved states list
        private void PlayStateAnimation(CharacterState characterState)
        {
            string stateName;

            switch (characterState)
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

            skeletonAnimator.PlayAnimationForState(stateName);
        }
    }
}