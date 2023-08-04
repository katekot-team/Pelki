using Pelki.Configs;

namespace Pelki.Gameplay.Input
{
    public class InputBySimpleInput : IInput
    {
        private readonly InputConfig inputConfig;

        public float Horizontal => SimpleInput.GetAxis(inputConfig.HorizontalAxisKey);
        public bool IsJump => SimpleInput.GetButton(inputConfig.JumpKey);

        public InputBySimpleInput(InputConfig inputConfig)
        {
            this.inputConfig = inputConfig;
        }
    }
}