using System;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        [SerializeField] private GameObject activatedState;
        [SerializeField] private GameObject notActivatedState;

        private bool isActivated;

        public event Action<SavePoint> Saved;

        private void Awake()
        {
            triggerDetector.Detected += OnDetected;
        }

        public void ActivateState()
        {
            triggerDetector.Detected -= OnDetected;
            isActivated = true;
            notActivatedState.SetActive(false);
            activatedState.SetActive(true);
        }

        public void NotActivateState()
        {
            isActivated = false;
            notActivatedState.SetActive(true);
            activatedState.SetActive(false);
        }

        private void OnDetected(GameObject player)
        {
            if (!isActivated)
            {
                Saved?.Invoke(this);
                ActivateState();
            }
        }
    }
}