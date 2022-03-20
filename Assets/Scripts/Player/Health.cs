using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Health : HealthBase<int>
    {
        [SerializeField] int maxArmour;
        [SerializeField] int initialLives;
        private int currentArmour, currentLives;
        public bool isFullHealth { get => currentHealth == maxHealth; }
        private void Start()
        {
            currentArmour = maxArmour;
            currentHealth = maxHealth;
            currentLives = initialLives;

            GameEvent.instance.ArmourhChanged(currentArmour);
            GameEvent.instance.HealthChanged(currentHealth);
            GameEvent.instance.LivesChanged(currentLives);
        }
        public void Add(int amount)
        {
            if (currentHealth >= maxHealth)
            {
                return;
            }

            currentHealth += amount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            GameEvent.instance.HealthChanged(currentHealth);
        }
        public void AddArmour(int amount)
        {
            if (currentArmour >= maxArmour)
            {
                return;
            }

            currentArmour += amount;
            if (currentArmour > maxArmour) currentArmour = maxArmour;
            GameEvent.instance.ArmourhChanged(currentArmour);
        }
        public override void Take(int amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
            GameEvent.instance.HealthChanged(currentHealth);

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

