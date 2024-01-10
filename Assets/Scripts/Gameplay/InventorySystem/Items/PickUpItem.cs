using System;
using UnityEngine;

namespace Pelki.Gameplay.InventorySystem.Items
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        
        //не смысловое название здесь связано с подбором. дело в том что "сохранение" это более высокоуровневая штука, 
        //а этому классу не надо знать, сохранили или ещё что сделали. его дело это сказать что его подобрали кому-то
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