using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Enemy
{
    public class Health : HealthBase<float>
    {
        [SerializeField] int score;
        [SerializeField] SpriteRenderer spriteRenderer;
        private EnemyAI enemyAI;
        private Collider healthBox;
        private void Start()
        {
            enemyAI = GetComponentInParent<EnemyAI>();
            healthBox = GetComponent<Collider>();
            currentHealth = maxHealth;
        }
        public override void Take(float amount)
        {
            currentHealth -= amount;
            spriteRenderer.color = Color.red;
            Invoke("NormalColor", .1f);
            if (currentHealth <= 0 && !isDead)
            {
                OnDeath();
            }
        }
        private void NormalColor()
        {
            spriteRenderer.color = Color.white;
        }
        public override void OnDeath()
        {
            enemyAI.OnDeath();
            isDead = true;
            healthBox.enabled = false;
            GameEvent.instance.ScoreChanged(score);
            enabled = false;
        }
    }
}
