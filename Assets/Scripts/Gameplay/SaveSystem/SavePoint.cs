using System;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        [SerializeField] private GameObject activatedState;
        [SerializeField] private GameObject notActivatedState;
        [SerializeField] private bool isActivated;

        public event Action<GameObject> Saved;

        private void Start()
        {
            triggerDetector.Detected += OnDetected;
            NotActivateState();
        }

        private void OnDetected(GameObject gameObject)
        {
            if (!isActivated)
            {
                Saved?.Invoke(gameObject);
                ActivateState();
            }
        }

        private void ActivateState()
        {
            isActivated = true;
            notActivatedState.SetActive(false);
            activatedState.SetActive(true);
        }

        private void NotActivateState()
        {
            isActivated = false;
            notActivatedState.SetActive(true);
            activatedState.SetActive(false);
        }
    }
}