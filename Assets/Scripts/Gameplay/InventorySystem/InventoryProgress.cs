using System.Collections.Generic;
using Pelki.Gameplay.InventorySystem.Items;
using Pelki.Gameplay.SaveSystem;
using UnityEngine;

namespace Pelki.Gameplay.InventorySystem
{
    public class InventoryProgress : BaseProgress<InventoryProgress>
    {
        private readonly List<string> _pickedUpPuzzleKeys = new List<string>();

        private IReadOnlyDictionary<string, PickUpItem> puzzleKeysRegister;
        
        private InventoryProgress() {}
        
        public IReadOnlyList<string> PickedUpPuzzleKeys => _pickedUpPuzzleKeys;

        public void Init(IReadOnlyDictionary<string, PickUpItem> puzzleKeysRegister)
        {
            this.puzzleKeysRegister = puzzleKeysRegister;
            foreach (var puzzleKeyItem in this.puzzleKeysRegister)
            {
                puzzleKeyItem.Value.PickedUp += OnPickedUp;
            }
        }
        
        private void OnPickedUp(PickUpItem pickUpItem)
        {
            if (puzzleKeysRegister == null)
            {
                return;
            }

            foreach (var (key, item) in puzzleKeysRegister)
            {
                if (pickUpItem.Equals(item))
                {
                    _pickedUpPuzzleKeys.Add(key);
                    Save();
                    item.PickedUp -= OnPickedUp;
                    Debug.Log("Save picked up item with ID: " + key);
                    item.Destroy();
                    return;
                }
            }
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
                    inventoryProgress.Initialize(_gameProgressStorage);
                }

                return inventoryProgress;
            }
        }
    }
}