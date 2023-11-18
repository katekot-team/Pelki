using Pelki.Frameworks.Spine;
using UnityEngine;

namespace Pelki.Gameplay.Characters.Animations
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SpineSkeletonAnimator _skeletonAnimator;

        private const string IdleName = "idle";
        private const string RunName = "run";
        private const string RiseName = "rise";
        private const string FallName = "fall";
        private const string AttackName = "attack";
        private const string RangedAttackName = "rangedAttack";

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
            _skeletonAnimator.PlayOneShot(AttackName, AttackTrackIndex);
        }

        public void PlayRangedAttack()
        {
            _skeletonAnimator.PlayOneShot(RangedAttackName, AttackTrackIndex);
        }

        private void PlayStateAnimation(MoverState moverState)
        {
            string stateName;

            switch (moverState)
            {
                case MoverState.Idle:
                    stateName = IdleName;
                    break;
                case MoverState.Run:
                    stateName = RunName;
                    break;
                case MoverState.Rise:
                    stateName = RiseName;
                    break;
                case MoverState.Fall:
                    stateName = FallName;
                    break;
                default:
                    return;
            }

            _skeletonAnimator.PlayAnimationForState(stateName);
        }
    }
}