using System.Collections.Generic;
using Pelki.Gameplay.InventorySystem.Items;
using UnityEngine;

namespace Pelki.Gameplay.InventorySystem
{
    public class Inventory
    {
        private InventoryProgress inventoryProgress;
        private IReadOnlyDictionary<string, PickUpItem> puzzleKeysRegister;
        
        public Inventory(InventoryProgress inventoryProgress, IReadOnlyDictionary<string, PickUpItem> puzzleKeysRegister)
        {
            this.inventoryProgress = inventoryProgress;
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
                    inventoryProgress.AddPuzzleKey(key);
                    inventoryProgress.Save();
                    item.PickedUp -= OnPickedUp;
                    Debug.Log("Save picked up item with ID: " + key);
                    item.Destroy();
                    return;
                }
            }
        }
    }
}