using System;
using UnityEngine;

namespace Pelki.Gameplay.InventorySystem.Items
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        
        public event Action<PickUpItem> PickedUp;
        
        private void Awake()
        {
            triggerDetector.Detected += OnDetected;
        }
        
        private void OnDetected(GameObject player)
        {
            PickedUp?.Invoke(this);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}