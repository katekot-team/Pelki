using Pelki.Configs;
using Pelki.UI;
using Pelki.UI.Screens;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class Game
    {
        private readonly LevelsConfig levelsConfig;

        private Level currentLevel;
        private ScreenSwitcher screenSwitcher;

        public Game(LevelsConfig levelsConfig, ScreenSwitcher screenSwitcher)
        {
            this.screenSwitcher = screenSwitcher;
            this.levelsConfig = levelsConfig;
        }

        public void StartGame()
        {
            Level levelPrefab = levelsConfig.DebugLevelPrefab;
            currentLevel = Object.Instantiate(levelPrefab);

            screenSwitcher.ShowScreen<GameScreen>();
        }
    }
}