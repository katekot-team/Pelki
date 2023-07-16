using UnityEngine;

namespace Pelki.Configs
{
    [CreateAssetMenu(fileName = nameof(MainSettingsConfig), menuName = "Configs/" + nameof(MainSettingsConfig), order = 0)]
    public class MainSettingsConfig : ScriptableObject
    {
        [SerializeField] private LevelsConfig levelsConfig;
        [SerializeField] private InputConfig inputConfig;
        [SerializeField] private CharactersConfig charactersConfig;

        public LevelsConfig LevelsConfig => levelsConfig;
        public InputConfig InputConfig => inputConfig;
        public CharactersConfig CharactersConfig => charactersConfig;
    }
}