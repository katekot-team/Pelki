using Pelki.Gameplay.Characters;
using UnityEngine;

namespace Pelki.Configs
{
    [CreateAssetMenu(fileName = nameof(CharactersConfig), menuName = "Configs/" + nameof(CharactersConfig), order = 0)]
    public class CharactersConfig : ScriptableObject
    {
        [SerializeField] private PlayerCharacter playerCharacterPrefab;

        public PlayerCharacter PlayerCharacterPrefab => playerCharacterPrefab;
    }
}