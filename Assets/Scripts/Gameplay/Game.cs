using Pelki.Configs;
using Pelki.Gameplay.Characters;
using Pelki.Gameplay.Input;
using Pelki.Gameplay.SaveSystem;
using Pelki.UI;
using Pelki.UI.Screens;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pelki.Gameplay
{
    public class Game
    {
        private readonly LevelsConfig _levelsConfig;
        private readonly ScreenSwitcher _screenSwitcher;
        private readonly IInput _input;
        private readonly CharactersConfig _charactersConfig;

        private Level _level;

        public Game(LevelsConfig levelsConfig, CharactersConfig charactersConfig, ScreenSwitcher screenSwitcher,
            IInput input)
        {
            _charactersConfig = charactersConfig;
            _input = input;
            _screenSwitcher = screenSwitcher;
            _levelsConfig = levelsConfig;
        }

        public void ThisUpdate()
        {
        }

        public void StartGame()
        {
            Level levelPrefab = _levelsConfig.DebugLevelPrefab;
            _level = Object.Instantiate(levelPrefab);

            foreach (var savePointItem in _level.SavePointsRegister)
            {
                savePointItem.Key.Saved += OnSaved;
            }

            PlayerCharacter playerCharacter = Object.Instantiate(_charactersConfig.PlayerCharacterPrefab,
                _level.CharacterSpawnPosition,
                Quaternion.identity, _level.transform);
            playerCharacter.Construct(_input);

            _screenSwitcher.ShowScreen<GameScreen>();
        }

        private void OnSaved(SavePoint savePoint)
        {
            // TODO сделать реализацию сохранения
            var savePointId = _level.SavePointsRegister[savePoint];
            Debug.Log("Save on savepoint with ID: " + savePointId);
        }
    }
}