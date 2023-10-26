using System;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        [SerializeField] private bool isActivated;

        public event Action<GameObject> Saved;

        private void Start()
        {
            triggerDetector.Detected += OnDetected;
        }

        private void OnDetected(GameObject gameObject)
        {
            if (!isActivated)
            {
                isActivated = true;
                Saved?.Invoke(gameObject);
            }
        }
    }
}