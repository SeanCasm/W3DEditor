using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Game.Collectibles;
using WEditor.Game.Scriptables;

namespace WEditor.Game.Enemy
{
    public class Health : HealthBase<float>
    {
        [SerializeField] int score;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] GameObject ammoPrefab;
        [SerializeField] List<CollectibleScriptable> ammoScriptables;
        private AudioSource audioSource;
        private EnemyAI enemyAI;
        public static float healthMultiplier = 1;
        private Collider healthBox;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            enemyAI = GetComponentInParent<EnemyAI>();
            healthBox = GetComponent<Collider>();
            currentHealth = maxHealth * healthMultiplier;
        }
        public override void Take(float amount)
        {
            currentHealth -= amount;
            spriteRenderer.color = Color.red;
            Invoke("NormalColor", .1f);
            enemyAI.HurtBehaviour();
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
            GameplayEvent.instance.KillsChanged();
            GameplayEvent.instance.ScoreChanged(score);

            enabled = false;
            audioSource.clip = deathClip[Random.Range(0, deathClip.Length)];
            audioSource.Play();

            GameObject ammoObject = Instantiate(ammoPrefab, transform.localPosition, Quaternion.identity, null);
            CollectibleBase collectible = ammoObject.GetComponent<CollectibleBase>();
            ammoScriptables[UnityEngine.Random.Range(0, 3)].LoadFromScriptable(collectible);
        }
    }
}
