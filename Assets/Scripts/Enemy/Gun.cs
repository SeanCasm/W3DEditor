using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Enemy
{
    public class Gun : GunBase<int>
    {
        [Range(0, 100)]
        [SerializeField] int accuracy;
        private EnemyAI enemyAI;
        private new void Start()
        {
            base.Start();
            enemyAI = GetComponent<EnemyAI>();
        }
        public void Fire()
        {
            audioSource.Play();
            AimToPlayer();
        }
        private void AimToPlayer()
        {
            var raycastHit = enemyAI.DrawRaycast();

            if (raycastHit.collider != null && raycastHit.collider.CompareTag("Player"))
            {
                int acc = UnityEngine.Random.Range(0, 100);
                if (acc > accuracy)
                    PlayerGlobalReference.instance.PlayerHealth.Take(damage);
            }
        }
    }
}
