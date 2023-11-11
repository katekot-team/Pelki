using Pelki.Configs;
using Pelki.Gameplay.Characters;
using Pelki.Gameplay.Input;
using Pelki.Gameplay.SaveSystem;
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
        private Progress levelProgress;

        public Game(LevelsConfig levelsConfig, CharactersConfig charactersConfig, ScreenSwitcher screenSwitcher,
            IInput input, Progress levelProgress)
        {
            this.charactersConfig = charactersConfig;
            this.input = input;
            this.screenSwitcher = screenSwitcher;
            this.levelsConfig = levelsConfig;
            this.levelProgress = levelProgress;
        }

        public void ThisUpdate()
        {
        }

        public void StartGame()
        {
            Level levelPrefab = levelsConfig.DebugLevelPrefab;
            level = Object.Instantiate(levelPrefab);
            foreach (var savePointItem in level.SavePointsRegister)
            {
                savePointItem.Key.Saved += OnSaved;
            }

            playerCharacter = Object.Instantiate(charactersConfig.PlayerCharacterPrefab,
                level.CharacterSpawnPosition,
                Quaternion.identity, level.transform);
            playerCharacter.Construct(input);

            screenSwitcher.ShowScreen<GameScreen>();
        }

        private void OnSaved(SavePoint savePoint)
        {
            var savePointId = level.SavePointsRegister[savePoint];
            if (levelProgress is LevelProgress progress)
            {
                progress.SavePointId = savePointId;
                progress.Save();
                Debug.Log("Save on savepoint with ID: " + savePointId);
            }
        }
    }
}