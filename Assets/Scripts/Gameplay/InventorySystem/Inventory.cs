using System.Collections.Generic;
using Pelki.Gameplay.InventorySystem.Items;
using Pelki.Gameplay.SaveSystem;

namespace Pelki.Gameplay.InventorySystem
{
    //давай постфикс добавим InventoryProgress, что бы понимать что это объект сохранения
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
                //кстати, очень прикольно. у нас получается так что PickUpItem не знает о том кому он отправляет событие
                //т.е. был вариант прокидывать в него инвентарь, но ты делаешь иначе. пытался найти проблему, но выглядит 
                //как хорошее решение, изолирующее puzzleKeysRegister от лишних данных
                puzzleKeyItem.Value.Saved += OnSaved;
            }
        }
        
        private void OnSaved(PickUpItem pickUpItem)
        {
            //может нам при подборе делать отписку? puzzleKeyItem.Value.Saved -= OnSaved; а то мало ли
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