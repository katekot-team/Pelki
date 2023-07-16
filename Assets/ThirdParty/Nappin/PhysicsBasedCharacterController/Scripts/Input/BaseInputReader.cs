using UnityEngine;

namespace PhysicsBasedCharacterController
{
    public abstract class BaseInputReader : MonoBehaviour
    {
        public abstract Vector2 AxisInput { get; protected set; }
        public abstract Vector2 CameraInput { get; protected set; }
        public abstract bool Jump { get; protected set; }
        public abstract bool JumpHold { get; protected set; }
        public abstract float Zoom { get; protected set; }
        public abstract bool Sprint { get; protected set; }
        public abstract bool Crouch { get; protected set; }
    }
}