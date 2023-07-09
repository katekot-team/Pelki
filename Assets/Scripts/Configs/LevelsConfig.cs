using System.Collections.Generic;
using UnityEngine;

namespace Pelki.Configs
{
    [CreateAssetMenu(fileName = nameof(LevelsConfig), menuName = "Configs/" + nameof(LevelsConfig), order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private Level debugLevel;
        //sttrox: когда будет несколько уровней
        //[SerializeField] private List<Level> levels;

        public Level DebugLevelPrefab => debugLevel;
    }
}