using UnityEngine;

namespace Pelki.Configs
{
    [CreateAssetMenu(fileName = nameof(MainSettingsConfig), menuName = "Configs/" + nameof(MainSettingsConfig), order = 0)]
    public class MainSettingsConfig : ScriptableObject
    {
        [SerializeField] private LevelsConfig levelsConfig;

        public LevelsConfig LevelsConfig => levelsConfig;
    }
}