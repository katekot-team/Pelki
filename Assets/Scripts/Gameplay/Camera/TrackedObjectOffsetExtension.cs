using Cinemachine;
using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public class TrackedObjectOffsetExtension : CinemachineExtension
    {
        [SerializeField] private CinemachineVirtualCamera _vcam;
        [SerializeField] private CinemachineFramingTransposer _transposer;
        [SerializeField] private float _flipRotationTime = 0.5f;
        [SerializeField] private float _cameraCenterOffsetX = 0.8f;

        private ICameraFollowByLookingAt _target;
        
        private void Reset()
        {
            _transposer = this.GetComponentInChildren<CinemachineFramingTransposer>();
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, 
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (_target == null)
            {
                return;
            }
            
            if (_target.IsLookingRight)
            {
                _transposer.m_TrackedObjectOffset.x = Mathf.Lerp(
                    _transposer.m_TrackedObjectOffset.x, 
                    _cameraCenterOffsetX, 
                    _flipRotationTime
                );
            }
            else
            {
                _transposer.m_TrackedObjectOffset.x = Mathf.Lerp(
                    _transposer.m_TrackedObjectOffset.x, 
                    -1 * _cameraCenterOffsetX,  
                    _flipRotationTime
                );
            }
        }

        public void SetTarget(ICameraFollowByLookingAt target)
        {
            _target = target;
        }
    }
}