using Pelki.Gameplay.Input;
using PhysicsBasedCharacterController;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterManager characterManager;
        [SerializeField] private PlayerCharacterInputReader inputReader;

        public void Construct(IInput input)
        {
            inputReader.Construct(input);
        }
    }
}