using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        
        public event Action<GameObject> Detected;
        
        public void Start()
        {
            Debug.Log("trigger detector start");
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("OnTriggerEnter");
            if ((layerMask.value & collider.gameObject.layer) == collider.gameObject.layer)
            {
                Detected?.Invoke(collider.GameObject());
            }
        }
    }
}