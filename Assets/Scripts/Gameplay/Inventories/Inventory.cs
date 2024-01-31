using Pelki.Gameplay.Inventories.Items;

namespace Pelki.Gameplay.Inventories
{
    public class Inventory
    {
        private readonly InventoryProgress _inventoryProgress;

        public Inventory(InventoryProgress inventoryProgress)
        {
            this._inventoryProgress = inventoryProgress;
        }

        public void AddItem(IItem item)
        {
            _inventoryProgress.AddPuzzleKey(item.Id);
            _inventoryProgress.Save();
        }

        public override string ToString()
        {
            var result = string.Join(";", _inventoryProgress.PickedUpPuzzleKeys);

            return result;
        }
    }
}