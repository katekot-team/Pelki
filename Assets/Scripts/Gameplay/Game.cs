using System.Linq;
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
        private LevelProgress levelProgress;

        public Game(LevelsConfig levelsConfig, CharactersConfig charactersConfig, ScreenSwitcher screenSwitcher,
            IInput input, LevelProgress progress)
        {
            this.charactersConfig = charactersConfig;
            this.input = input;
            this.screenSwitcher = screenSwitcher;
            this.levelsConfig = levelsConfig;
            this.levelProgress = progress;
        }

        public void ThisUpdate()
        {
        }

        public void StartGame()
        {
            Level levelPrefab = levelsConfig.DebugLevelPrefab;
            level = Object.Instantiate(levelPrefab);
            Vector3 spawnPosition = level.CharacterSpawnPosition;
            SavePoint savePoint = level.SavePointsRegister[levelProgress.SavePointId];
            spawnPosition = savePoint.transform.position;
            /*savePoint.ActivateState();
            savePoint.Saved -= OnSaved;*/
            foreach (var savePointItem in level.SavePointIdsRegister)
            {
                if (savePointItem.Key.Equals(savePoint))
                {
                    savePoint.ActivateState();
                    savePoint.Saved -= OnSaved;
                }
                else
                {
                    savePointItem.Key.Construct();
                    savePointItem.Key.Saved += OnSaved;
                }
            }
            
            playerCharacter = Object.Instantiate(charactersConfig.PlayerCharacterPrefab,
                spawnPosition,
                Quaternion.identity, level.transform);
            playerCharacter.Construct(input);

            screenSwitcher.ShowScreen<GameScreen>();
        }

        private void OnSaved(SavePoint savePoint)
        {
            var savePointId = level.SavePointIdsRegister[savePoint];
            levelProgress.SavePointId = savePointId;
            levelProgress.Save();
            Debug.Log("Save on savepoint with ID: " + savePointId);
        }
    }
}