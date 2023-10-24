using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        
        public event Action<GameObject> Detected;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == layerMask.value)
            {
                Detected?.Invoke(collider.GameObject());
            }
        }
    }
}