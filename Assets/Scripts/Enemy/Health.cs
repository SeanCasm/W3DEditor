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
        public static float healthMultiplier = 1;
        private Collider healthBox;
        private void Start()
        {
            enemyAI = GetComponentInParent<EnemyAI>();
            healthBox = GetComponent<Collider>();
            currentHealth = maxHealth * healthMultiplier;
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
            GameplayEvent.instance.ScoreChanged(score);
            enabled = false;
        }
    }
}
