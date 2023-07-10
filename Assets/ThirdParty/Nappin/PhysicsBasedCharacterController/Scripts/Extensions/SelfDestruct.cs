using System.Collections;
using UnityEngine;


namespace PhysicsBasedCharacterController
{
    public class SelfDestruct : MonoBehaviour
    {
        [Header("Self destruction parameters")]
        public float timeDelay = 2f;


        private void Start()
        {
            StartCoroutine(WaitSeconds(timeDelay));
        }


        private IEnumerator WaitSeconds(float _time)
        {
            yield return new WaitForSeconds(_time);
            GameObject.Destroy(this.gameObject);
        }
    }
}