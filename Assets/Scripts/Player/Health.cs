using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Health : HealthBase<int>, IFullable
    {
        public bool isImmortal { get; set; }
        /// <summary>
        /// Cheks if the current amount of health is equal to the max amount of health
        /// </summary>
        public bool isFullOfAmmo => currentHealth == maxHealth;

        private void OnEnable()
        {
            currentHealth = maxHealth;
            GameplayEvent.instance.HealthChanged(currentHealth);
        }
        public bool Add(int amount)
        {
            if (currentHealth >= maxHealth)
            {
                return false;
            }

            currentHealth += amount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;


            GameplayEvent.instance.HealthChanged(currentHealth);
            return true;
        }
        public override void Take(int amount)
        {
            if (isImmortal) return;
            WEditor.UI.PlayerDamage.instance.StartAnimation();
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
            GameplayEvent.instance.HealthChanged(currentHealth);

            if (currentHealth <= 0)
                OnDeath();
        }

        public override void OnDeath()
        {
            GameplayEvent.instance.OnDeath();
            Time.timeScale = 0;
        }
    }
}