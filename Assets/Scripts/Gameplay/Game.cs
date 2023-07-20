using Pelki.Configs;
using Pelki.Gameplay.Characters;
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
        private readonly CharactersConfig charactersConfig;

        private Level level;
        private PlayerCharacter playerCharacter;

        public Game(LevelsConfig levelsConfig, CharactersConfig charactersConfig, ScreenSwitcher screenSwitcher,
            IInput input)
        {
            this.charactersConfig = charactersConfig;
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
            level = Object.Instantiate(levelPrefab);

            playerCharacter = Object.Instantiate(charactersConfig.PlayerCharacterPrefab,
                level.CharacterSpawnPosition,
                Quaternion.identity, level.transform);
            playerCharacter.Construct(input);

            screenSwitcher.ShowScreen<GameScreen>();
        }
    }
}