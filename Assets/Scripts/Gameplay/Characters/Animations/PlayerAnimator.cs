using Pelki.Frameworks.Spine;
using UnityEngine;

namespace Pelki.Gameplay.Characters.Animations
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SpineSkeletonAnimator _skeletonAnimator;

        private const string IdleHash = "idle";
        private const string RunHash = "run";
        private const string RiseHash = "rise";
        private const string FallHash = "fall";
        private const string AttackHash = "attack";
        private const string RangedAttackHash = "rangedAttack";

        private const int AttackTrackIndex = 2;

        private MoverState _previousState;
        private MoverState _currentState;

        public void Initialize()
        {
            _skeletonAnimator.Initialize();
        }

        public void SetFlip(float inputHorizontal)
        {
            _skeletonAnimator.SetFlip(inputHorizontal);
        }

        public void SetState(MoverState moverState)
        {
            _currentState = moverState;

            if (_previousState == _currentState)
            {
                return;
            }

            _previousState = _currentState;

            PlayStateAnimation(moverState);
        }

        public void PlayMeleeAttack()
        {
            _skeletonAnimator.PlayOneShot(AttackHash, AttackTrackIndex);
        }

        public void PlayRangedAttack()
        {
            _skeletonAnimator.PlayOneShot(RangedAttackHash, AttackTrackIndex);
        }

        private void PlayStateAnimation(MoverState moverState)
        {
            string stateName;

            switch (moverState)
            {
                case MoverState.Idle:
                    stateName = IdleHash;
                    break;
                case MoverState.Run:
                    stateName = RunHash;
                    break;
                case MoverState.Rise:
                    stateName = RiseHash;
                    break;
                case MoverState.Fall:
                    stateName = FallHash;
                    break;
                default:
                    return;
            }

            _skeletonAnimator.PlayAnimationForState(stateName);
        }
    }
}