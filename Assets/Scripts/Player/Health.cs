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
        public bool isImmortal { get; set; }
        public bool isFullHealth { get => currentHealth == maxHealth; }
        private void OnEnable()
        {
            currentArmour = maxArmour;
            currentHealth = maxHealth;
            currentLives = initialLives;

            GameplayEvent.instance.ArmourhChanged(currentArmour);
            GameplayEvent.instance.HealthChanged(currentHealth);
            GameplayEvent.instance.LivesChanged(currentLives);
        }
        public void Add(int amount)
        {
            if (currentHealth >= maxHealth)
            {
                return;
            }

            currentHealth += amount;
            if (currentHealth > maxHealth) currentHealth = maxHealth;


            GameplayEvent.instance.HealthChanged(currentHealth);
        }
        public void AddArmour(int amount)
        {
            if (currentArmour >= maxArmour)
            {
                return;
            }

            currentArmour += amount;
            if (currentArmour > maxArmour) currentArmour = maxArmour;
            GameplayEvent.instance.ArmourhChanged(currentArmour);
        }
        public override void Take(int amount)
        {
            if (isImmortal) return;
            WEditor.UI.PlayerDamage.instance.StartAnimation();
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
            GameplayEvent.instance.HealthChanged(currentHealth);

            if (currentHealth <= 0 && currentLives <= 0)
            {
                OnDeath();
                return;
            }
            else if (currentHealth <= 0 && currentLives > 0)
            {
                currentLives--;
                currentHealth = maxHealth;
                currentArmour = maxArmour;
                GameplayEvent.instance.HealthChanged(currentHealth);
                GameplayEvent.instance.ArmourhChanged(currentArmour);
                GameplayEvent.instance.LivesChanged(currentLives);
                StatusBehaviour.instance.Respawn();
            }
        }

        public override void OnDeath()
        {
            Time.timeScale = 0;
            WEditor.UI.DeathMenu.instance.Enable();
        }
    }
}