using Cinemachine;
using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public class CameraDistributor : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        public void Init(ICameraFollowByLookingAt target)
        {
            var trackedObjectOffsetExtension = _virtualCamera.GetComponentInChildren<TrackedObjectOffsetExtension>();
            trackedObjectOffsetExtension.SetTarget(target);
        }
    }
}
