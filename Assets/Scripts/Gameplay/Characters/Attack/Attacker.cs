using System;
using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters.Attack
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private ProjectileSpawner _projectileSpawner;
        [SerializeField] private float _attackCooldown;

        private IInput _input;
        private float _reloadCompletionTime;
        private bool _canPerformRangedAttack;

        public event Action InvokedRangeAttack;

        public void Construct(IInput input)
        {
            _input = input;
        }

        private void Update()
        {
            if (!_canPerformRangedAttack && Time.time > _reloadCompletionTime)
            {
                _canPerformRangedAttack = true;
            }

            if (IsPerformingRangedAttack())
            {
                RangedAttack();
            }
        }

        private bool IsPerformingRangedAttack()
        {
            bool isCalled = _input.IsRangedAttacking;

            return isCalled && _canPerformRangedAttack;
        }

        private void RangedAttack()
        {
            _projectileSpawner.Shoot(transform.right);
            _canPerformRangedAttack = false;
            _reloadCompletionTime = Time.time + _attackCooldown;
            InvokedRangeAttack?.Invoke();
        }
    }
}