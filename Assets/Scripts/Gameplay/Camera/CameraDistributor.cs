using Cinemachine;
using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public class CameraDistributor : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private TrackedObjectOffsetExtension _trackedObjectOffsetExtension;

        public void SetTargetFollow(ICameraFollowByLookingAt target)
        {
            _virtualCamera.Follow = target.FollowRoot;
            _trackedObjectOffsetExtension.SetTarget(target);
        }
    }
}