using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : Entity
    {
        private IInput input;

        public void Construct(IInput input)
        {
            this.input = input;
        }
    }
}