using UnityEngine;


namespace PhysicsBasedCharacterController
{
    public class VFXManager : MonoBehaviour
    {
        [Header("Particle references")]
        public CharacterManager characterManager;
        [Space(10)]

        public GameObject particleJump;
        public GameObject particleLand;
        public GameObject particleFast;
        [Space(10)]

        public bool enableVFX = false;

        private CapsuleCollider _collider;
        private GameObject characterModel;


        /**/


        private void Awake()
        {
            _collider = characterManager.GetComponent<CapsuleCollider>();
            characterModel = characterManager.characterModel;
        }


        #region VFX 

        public void ParticleJump()
        {
            if (enableVFX)
            {
                GameObject tmpObj = GameObject.Instantiate(particleJump, characterManager.transform.position - new Vector3(0f, _collider.height / 2f, 0f), Quaternion.identity);
                tmpObj.transform.parent = this.transform;
            }
        }

        public void ParticleLand()
        {
            if (enableVFX)
            {
                GameObject tmpObj = GameObject.Instantiate(particleLand, characterManager.transform.position - new Vector3(0f, _collider.height / 2f, 0f), Quaternion.identity);
                tmpObj.transform.parent = this.transform;
            }
        }

        public void ParticleFast()
        {
            if (enableVFX)
            {
                GameObject tmpObj = GameObject.Instantiate(particleFast, characterManager.transform.position, characterModel.transform.rotation);
                tmpObj.transform.parent = characterManager.transform;
            }
        }

        #endregion
    }
}