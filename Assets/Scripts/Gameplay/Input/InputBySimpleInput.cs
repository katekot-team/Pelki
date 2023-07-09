using Pelki.Configs;

namespace Pelki.Gameplay.Input
{
    public class InputBySimpleInput : IInput
    {
        private InputConfig inputConfig;

        public float Horizontal => SimpleInput.GetAxis(inputConfig.HorizontalAxisKey);

        public InputBySimpleInput(InputConfig inputConfig)
        {
            this.inputConfig = inputConfig;
        }
    }
}