using Pelki.Configs;
using UnityEngine;

namespace Pelki.Gameplay.Input
{
    public class InputBySimpleInputAndKeyboard : IInput
    {
        public float Horizontal => GetHorizontal();
        public bool IsJump => GetHasJump();
        public bool IsRangedAttacking => GetHasRangedAttack();

        private readonly InputBySimpleInput simpleInput;
        private readonly InputConfig inputConfig;

        public InputBySimpleInputAndKeyboard(InputConfig inputConfig)
        {
            this.inputConfig = inputConfig;
            simpleInput = new InputBySimpleInput(inputConfig);
        }

        private float GetHorizontal()
        {
            float result = 0;
            float pcHorizontal = UnityEngine.Input.GetAxis(inputConfig.HorizontalAxisKey);
            result = pcHorizontal;

            if (pcHorizontal == 0)
            {
                result = simpleInput.Horizontal;
            }

            return result;
        }

        private bool GetHasJump()
        {
            bool isJump = false;
            isJump = UnityEngine.Input.GetButton(inputConfig.JumpKey);
            if (!isJump)
            {
                isJump = simpleInput.IsJump;
            }

            return isJump;
        }

        private bool GetHasRangedAttack()
        {
            bool isAttacking = false;
            isAttacking =
                UnityEngine.Input.GetButton(inputConfig.RangedAttackKey)
                || UnityEngine.Input.GetKeyDown(KeyCode.F);
            if (!isAttacking)
            {
                isAttacking = simpleInput.IsRangedAttacking;
            }

            return isAttacking;
        }
    }
}