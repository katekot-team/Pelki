using System;
using System.Collections;
using Pelki.Gameplay.Characters;
using UnityEngine;

namespace Pelki.Gameplay.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private float _flipRotationTime = 0.5f;

        private Coroutine _turnCoroutine;
        private PlayerCharacter _player;
        private bool _isFacingRight;

        private void Update()
        {
            if (_player)
            {
                transform.position = _player.transform.position;
            }
        }

        public void Init(PlayerCharacter playerCharacter)
        {
            _player = playerCharacter;
            _isFacingRight = _player.isFacingRight;
        }

        public void CallTurn()
        {
            _turnCoroutine = StartCoroutine(FlipYLerp());
        }

        private IEnumerator FlipYLerp()
        {
            float startRotation = transform.localEulerAngles.y;
            float endRotationAmount = DetermineEndRotation();
            float yRotation = 0f;

            float elapsedTime = 0f;
            while (elapsedTime < _flipRotationTime)
            {
                elapsedTime += Time.deltaTime;
                
                // lerp the y rotation
                yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipRotationTime));
                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

                yield return null;
            }
        }

        private float DetermineEndRotation()
        {
            _isFacingRight = !_isFacingRight;
            if (_isFacingRight)
            {
                return -160f;
            }
            
            return 0f;
        }
    }
}