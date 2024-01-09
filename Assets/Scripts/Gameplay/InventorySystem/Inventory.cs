using System.Collections.Generic;
using Pelki.Gameplay.InventorySystem.Items;
using Pelki.Gameplay.SaveSystem;

namespace Pelki.Gameplay.InventorySystem
{
    public class Inventory : BaseProgress<Inventory>
    {
        private readonly List<string> _pickedUpPuzzleKeys = new List<string>();

        private IReadOnlyDictionary<string, PickUpItem> puzzleKeysRegister;
        
        public IReadOnlyList<string> PickedUpPuzzleKeys => _pickedUpPuzzleKeys;

        public void Init(IReadOnlyDictionary<string, PickUpItem> puzzleKeysRegister)
        {
            this.puzzleKeysRegister = puzzleKeysRegister;
            foreach (var puzzleKeyItem in this.puzzleKeysRegister)
            {
                puzzleKeyItem.Value.Saved += OnSaved;
            }
        }
        
        private void OnSaved(PickUpItem pickUpItem)
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
                    return;
                }
            }
        }
    }
}