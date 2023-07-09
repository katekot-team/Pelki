using Pelki.Configs;
using Pelki.Gameplay.Input;
using Pelki.UI;
using Pelki.UI.Screens;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class Game
    {
        private readonly LevelsConfig levelsConfig;
        private readonly ScreenSwitcher screenSwitcher;
        private readonly IInput input;

        private Level currentLevel;

        public Game(LevelsConfig levelsConfig, ScreenSwitcher screenSwitcher, IInput input)
        {
            this.input = input;
            this.screenSwitcher = screenSwitcher;
            this.levelsConfig = levelsConfig;
        }

        public void ThisUpdate()
        {
        }

        public void StartGame()
        {
            Level levelPrefab = levelsConfig.DebugLevelPrefab;
            currentLevel = Object.Instantiate(levelPrefab);

            screenSwitcher.ShowScreen<GameScreen>();
        }
    }
}