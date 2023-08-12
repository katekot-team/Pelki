using Pelki.Gameplay.Input;
using UnityEngine;

namespace Pelki.Gameplay.Characters
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private ProjectileSpawner projectileSpawner;
        [SerializeField] private float attackCooldown;
        [SerializeField] private KeyCode rangedAttackPC;

        private float reloadCompletionTime;
        private bool canPerformRangedAttack = true;
        private IInput input;

        public void Construct(IInput input)
        {
            this.input = input;
        }

        private void Update()
        {
            if (!canPerformRangedAttack && Time.time > reloadCompletionTime)
            {
                canPerformRangedAttack = true;
            }

            if (IsPerformingRangedAttack())
            {
                RangedAttack();
            }
        }

        private bool IsPerformingRangedAttack()
        {
            bool isCalled = input.IsRangedAttacking ||
                            UnityEngine.Input.GetKeyDown(rangedAttackPC);

            return isCalled && canPerformRangedAttack;
        }

        private void RangedAttack()
        {
            projectileSpawner.Shoot(Vector2.right);
            canPerformRangedAttack = false;
            reloadCompletionTime = Time.time + attackCooldown;
        }
    }
}