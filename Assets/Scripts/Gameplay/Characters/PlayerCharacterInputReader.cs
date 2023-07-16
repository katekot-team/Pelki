using Pelki.Gameplay.Input;
using PhysicsBasedCharacterController;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacterInputReader : BaseInputReader
    {
        private IInput input;

        public override Vector2 AxisInput { get; protected set; }
        public override Vector2 CameraInput { get; protected set; }
        public override bool Jump { get; protected set; }
        public override bool JumpHold { get; protected set; }
        public override float Zoom { get; protected set; }
        public override bool Sprint { get; protected set; }
        public override bool Crouch { get; protected set; }

        public void Construct(IInput input)
        {
            this.input = input;
        }

        private void Update()
        {
            AxisInput = new Vector2(input.Horizontal, 0);
        }
    }
}