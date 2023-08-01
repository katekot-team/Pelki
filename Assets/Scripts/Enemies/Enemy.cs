using NaughtyAttributes;
using Pelki.Interfaces;
using UnityEngine;

namespace Pelki.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField, MinValue(0)] private int maxHealth;
        public int Health { get; private set; }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 1)
            {
                Death();
            }
            Debug.Log("Current health is " + Health);
        }
        
        private void Start()
        {
            Health = maxHealth;
        }
        
        private void Death()
        {
            
        }
    }
}
