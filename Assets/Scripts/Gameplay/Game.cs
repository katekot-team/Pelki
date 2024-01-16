using System.Linq;
using Pelki.Configs;
using Pelki.Gameplay.Camera;
using Pelki.Gameplay.Characters;
using Pelki.Gameplay.Input;
using Pelki.Gameplay.InventorySystem;
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
        //у нас в Main уже есть собственный _gameProgressStorage
        private readonly GameProgressStorage _gameProgressStorage = new GameProgressStorage();

        private Level _level;
        private LevelProgress _levelProgress;
        private CameraDistributor _cameraDistributor;
        private PlayerCharacter _playerCharacter;
        private InventoryProgress _inventoryProgress;

        public Game(LevelsConfig levelsConfig, CharactersConfig charactersConfig, ScreenSwitcher screenSwitcher,
            IInput input, LevelProgress progress, CameraDistributor cameraDistributor)
        {
            _charactersConfig = charactersConfig;
            _input = input;
            _screenSwitcher = screenSwitcher;
            _levelsConfig = levelsConfig;
            _levelProgress = progress;
            _cameraDistributor = cameraDistributor;
        }

        public void ThisUpdate()
        {
        }

        public void StartGame()
        {
            Level levelPrefab = _levelsConfig.DebugLevelPrefab;
            _level = Object.Instantiate(levelPrefab);
            SavePoint spawnSavePoint = _level.SavePointsRegister[_levelProgress.LastSavePointId];
            Vector3 spawnPosition = spawnSavePoint.transform.position;
            foreach (var savePointItem in _level.SavePointIdsRegister)
            {
                if (savePointItem.Key.Equals(spawnSavePoint)
                    //sttrox: ActivatedSavePoints является list, что не супер оптимизированно, но она вроде как тут и не нужна
                    && _levelProgress.ActivatedSavePoints.Contains(savePointItem.Value))
                {
                    spawnSavePoint.ActivateState();
                }
                else
                {
                    savePointItem.Key.NotActivateState();
                    savePointItem.Key.Saved += OnSaved;
                }
            }
            //у нас так уж получилось что вот этой задаче по загрущке сохранений занимается main,
            //давай, пусть он и продолжит этим заниматься. 
            //+я сдел пример на фабрике, для levelProgress, сделай по аналогии фабрику для _inventoryProgress
            //фабрики, по тому что иначе у нас в main будет слишком много логики, не думаю что это хорошо, пусть специчные классы этим займутся
            if (_gameProgressStorage.TryLoadGameProgress(out _inventoryProgress) == false)
            {
                Debug.Log("Inventory is empty");
                _inventoryProgress = new InventoryProgress();
                _inventoryProgress.Initialize(_gameProgressStorage);
            }
            _inventoryProgress.Init(_level.PuzzleKeysRegister);
            foreach (var pickUpPuzzleKey in _inventoryProgress.PickedUpPuzzleKeys)
            {
                if (_level.PuzzleKeysRegister.ContainsKey(pickUpPuzzleKey))
                {
                    _level.PuzzleKeysRegister[pickUpPuzzleKey].Destroy();
                }
            }

            _playerCharacter = Object.Instantiate(_charactersConfig.PlayerCharacterPrefab,
                spawnPosition,
                Quaternion.identity, _level.transform);
            _playerCharacter.Construct(_input, _inventoryProgress);

            _cameraDistributor.SetTargetFollow(_playerCharacter);

            _screenSwitcher.ShowScreen<GameScreen>();
        }

        private void OnSaved(SavePoint savePoint)
        {
            var savePointId = _level.SavePointIdsRegister[savePoint];
            _levelProgress.AddActivatedSavePoint(savePointId);
            _levelProgress.Save();
            Debug.Log("Save on savepoint with ID: " + savePointId);
        }
    }
}