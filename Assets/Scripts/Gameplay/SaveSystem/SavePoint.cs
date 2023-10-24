using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Pelki.Gameplay.SaveSystem
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private TriggerDetector triggerDetector;
        
        public event Action<GameObject> Saved;

        public bool isActivated = false;

        public void Start()
        {
            triggerDetector.Detected += OnDetected;
        }

        public void OnDetected(GameObject gameObject)
        {
            Debug.Log("On Detected");
            if (!isActivated)
            {
                isActivated = true;
                Saved?.Invoke(gameObject);
            }
        }
    }
}