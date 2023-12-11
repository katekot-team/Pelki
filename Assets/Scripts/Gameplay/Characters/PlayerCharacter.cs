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

        public bool isFacingRight => _isFacingRight;

        public void Construct(IInput input)
        {
            _input = input;

            _mover.Construct(input);
            _attacker.Construct(input);

            _playerAnimator.Initialize();
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
            }
            else if (_input.Horizontal < 0f && _isFacingRight)
            {
                _isFacingRight = false;
            }
            _playerAnimator.SetFlip(_input.Horizontal);
            _playerAnimator.SetState(_mover.CurrentState);
        }
    }
}