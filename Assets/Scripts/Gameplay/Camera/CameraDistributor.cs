using Cinemachine;
using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public class CameraDistributor : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float _flipRotationTime = 0.5f;
        [SerializeField] private float _cameraCenterOffsetX = 0.8f;

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;
        public float FlipRotationTime => _flipRotationTime;
        public float CameraCenterOffsetX => _cameraCenterOffsetX;
    }
}
