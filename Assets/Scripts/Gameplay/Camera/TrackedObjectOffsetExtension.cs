using Cinemachine;
using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public class TrackedObjectOffsetExtension : CinemachineExtension
    {
        [SerializeField] private CinemachineFramingTransposer _transposer;
        [SerializeField] private float _flipRotationDuration = 1.5f;
        [SerializeField] private float _flipRotationMaxSpeed = 1;
        [SerializeField] private float _cameraCenterOffsetX = 0.8f;

        private ICameraFollowByLookingAt _target;

        private float _currentFlipRotationVelocity = 0;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (_target == null)
            {
                return;
            }

            float currentX = _transposer.m_TrackedObjectOffset.x;
            float targetOffsetX = _target.IsLookingRight ? _cameraCenterOffsetX : -1 * _cameraCenterOffsetX;

            float newX = Mathf.SmoothDamp(currentX, targetOffsetX, ref _currentFlipRotationVelocity,
                _flipRotationDuration, _flipRotationMaxSpeed, deltaTime);

            _transposer.m_TrackedObjectOffset.x = newX;
        }

        public void SetTarget(ICameraFollowByLookingAt target)
        {
            _target = target;

            _currentFlipRotationVelocity = target.IsLookingRight ? 1 : -1;
        }

        private void Reset()
        {
            _transposer = this.GetComponentInChildren<CinemachineFramingTransposer>();
        }
    }
}