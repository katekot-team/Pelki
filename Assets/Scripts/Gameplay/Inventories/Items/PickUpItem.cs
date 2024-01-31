using UnityEngine;

namespace Pelki.Gameplay.Inventories.Items
{
    public class PickUpItem : MonoBehaviour, IItem
    {
        private string _id;

        public string Id => _id;

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