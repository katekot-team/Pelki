using System;
using UnityEngine;

namespace Pelki.Gameplay.InventorySystem.Items
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        
        public event Action<PickUpItem> Saved;
        
        private void Awake()
        {
            triggerDetector.Detected += OnDetected;
        }
        
        private void OnDetected(GameObject player)
        {
            Saved?.Invoke(this);
            Destroy(this);
        }
    }
}