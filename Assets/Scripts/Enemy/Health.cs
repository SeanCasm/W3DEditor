using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Enemy
{
    public class Health : HealthBase<float>
    {
        [SerializeField] int score;
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
            GameEvent.instance.ScoreChanged(score);
        }
    }
}
