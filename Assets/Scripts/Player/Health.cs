using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Player
{
    public class Health : HealthBase<int>
    {
        [SerializeField] int maxArmour;
        private int currentArmour;
        public bool isFullHealth { get => currentHealth == maxHealth; }
        public void Add(int amount)
        {
            if (currentHealth >= maxHealth)
            {
                return;
            }
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
        public void AddArmour(int amount)
        {
            if (currentArmour >= maxArmour)
            {
                return;
            }
        }
        public override void Take(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                OnDeath();
            }
        }

        public override void OnDeath()
        {
            //death screen
        }
    }
}

