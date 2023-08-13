using Pelki.Gameplay.Characters.Movements;
using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private GroundMover mover;

        private IInput input;

        public void Construct(IInput input)
        {
            this.input = input;
            mover.Construct(input);
        }
    }
}