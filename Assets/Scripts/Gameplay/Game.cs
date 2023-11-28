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
        private readonly LevelsConfig _levelsConfig;
        private readonly ScreenSwitcher _screenSwitcher;
        private readonly IInput _input;
        private readonly CharactersConfig _charactersConfig;

        private Level _level;
        private PlayerCharacter playerCharacter;
        private LevelProgress _levelProgress;

        public Game(LevelsConfig levelsConfig, CharactersConfig charactersConfig, ScreenSwitcher screenSwitcher,
            IInput input, LevelProgress progress)
        {
            _charactersConfig = charactersConfig;
            _input = input;
            _screenSwitcher = screenSwitcher;
            _levelsConfig = levelsConfig;
            _levelProgress = progress;
        }

        public void ThisUpdate()
        {
        }

        public void StartGame()
        {
            Level levelPrefab = _levelsConfig.DebugLevelPrefab;
            _level = Object.Instantiate(levelPrefab);
            Vector3 spawnPosition = _level.CharacterSpawnPosition;
            SavePoint savePoint = _level.SavePointsRegister[levelProgress.SavePointId];
            spawnPosition = savePoint.transform.position;

            foreach (var savePointItem in _level.SavePointIdsRegister)
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
            
            playerCharacter = Object.Instantiate(_charactersConfig.PlayerCharacterPrefab,
                spawnPosition,
                Quaternion.identity, _level.transform);
            playerCharacter.Construct(_input);

            _screenSwitcher.ShowScreen<GameScreen>();
        }

        private void OnSaved(SavePoint savePoint)
        {
            var savePointId = _level.SavePointIdsRegister[savePoint];
            _levelProgress.SavePointId = savePointId;
            _levelProgress.Save();
            Debug.Log("Save on savepoint with ID: " + savePointId);
        }
    }
}