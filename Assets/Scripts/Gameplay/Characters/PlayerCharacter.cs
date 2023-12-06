using Cinemachine;
using Pelki.Gameplay.Camera;
using Pelki.Gameplay.Characters.Animations;
using Pelki.Gameplay.Characters.Attack;
using Pelki.Gameplay.Characters.Movements;
using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private GroundMover _mover;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Attacker _attacker;

        private IInput _input;
        private bool _isFacingRight = true;
        private CameraFollower _cameraFollower;
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineFramingTransposer _framingTransposer;

        public bool isFacingRight => _isFacingRight;

        public void Construct(IInput input, CameraFollower cameraFollower)
        {
            _input = input;

            _mover.Construct(input);
            _attacker.Construct(input);

            _playerAnimator.Initialize();

            _cameraFollower = cameraFollower;
        }

        private void OnEnable()
        {
            _attacker.InvokedRangeAttack += _playerAnimator.PlayRangedAttack;
        }

        private void OnDisable()
        {
            _attacker.InvokedRangeAttack -= _playerAnimator.PlayRangedAttack;
        }

        private void Update()
        {
            if (_input.Horizontal > 0f && !_isFacingRight)
            {
                _isFacingRight = true;
                _cameraFollower.CallTurn();
            }
            else if (_input.Horizontal < 0f && _isFacingRight)
            {
                _isFacingRight = false;
                _cameraFollower.CallTurn();
            }
            _playerAnimator.SetFlip(_input.Horizontal);
            _playerAnimator.SetState(_mover.CurrentState);
        }
    }
}