using System;
using UnityEngine;

namespace Pelki.Gameplay
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;

        public event Action Detected;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if ((layerMask & (1 << collider.gameObject.layer)) != 0)
            {
                Detected?.Invoke();
            }
        }
    }
}