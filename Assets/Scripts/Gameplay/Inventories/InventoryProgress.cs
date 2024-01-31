using System.Collections.Generic;
using Pelki.Gameplay.SaveSystem;
using UnityEngine;

namespace Pelki.Gameplay.Inventories
{
    public class InventoryProgress : BaseProgress<InventoryProgress>
    {
        private readonly List<string> _pickedUpPuzzleKeys = new List<string>();

        private InventoryProgress() {}
        
        public IReadOnlyList<string> PickedUpPuzzleKeys => _pickedUpPuzzleKeys;

        public void AddPuzzleKey(string key)
        {
            _pickedUpPuzzleKeys.Add(key);
        }
        
        public class Factory
        {
            private GameProgressStorage _gameProgressStorage;

            public Factory(GameProgressStorage gameProgressStorage)
            {
                _gameProgressStorage = gameProgressStorage;
            }

            public InventoryProgress Create(Level currentLevel)
            {
                InventoryProgress inventoryProgress;
                if (_gameProgressStorage.TryLoadGameProgress(out inventoryProgress) == false)
                {
                    Debug.Log("Inventory is empty");
                    inventoryProgress = new InventoryProgress();
                }
                inventoryProgress.Initialize(_gameProgressStorage);

                return inventoryProgress;
            }
        }
    }
}