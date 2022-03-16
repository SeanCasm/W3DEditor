using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;

namespace WEditor.Game.Enemy
{
    public class Health : HealthBase<float>
    {
        public override void Take(float amount)
        {
            currentHealth -= amount;
            if (currentHealth == 0 && !isDead)
            {
                OnDeath();
            }
        }
        public override void OnDeath()
        {
            isDead = true;
            // Drop...
        }
    }
}
