using UnityEngine;

//todo rename to InventorySystem -> Invintories? 
namespace Pelki.Gameplay.InventorySystem.Items
{
    public class PickUpItem : MonoBehaviour, IItem
    {
        private string _id;

        public string Id => _id;

        //todo invioke
        public void Initialize(string id)
        {
            _id = id;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}