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

        public void Initialize()
        {
            skeletonAnimator.Initialize();
        }

        public void SetFlip(float inputHorizontal) =>
            skeletonAnimator.SetFlip(inputHorizontal);

        public void UpdateState(CharacterState characterState)
        {
            HandleStateChanged(characterState);
        }

        public void PlayRangedAttack()
        {
            skeletonAnimator.PlayOneShot(RangedAttackHash, lockStateChanging: false, trackIndex: 1);
        }

        //klavikus: require change according to approved states list
        private void HandleStateChanged(CharacterState characterState)
        {
            string stateName;

            //klavikus: Idea -> change to dictionary<State, Hash>
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

            Debug.Log(stateName);
            skeletonAnimator.PlayAnimationForState(stateName, 0);
        }
    }
}