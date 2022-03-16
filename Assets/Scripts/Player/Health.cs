using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Player
{
    public class Health : HealthBase<int>
    {
        public void Add(int amount)
        {
            if (currentHealth >= maxHealth)
            {
                return;
            }
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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

