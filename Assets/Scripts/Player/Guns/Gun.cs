using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
namespace WEditor.Game.Player
{
    public class Gun : GunBase<float>
    {
        protected Animator animator;
        public bool isShooting { get; private set; }
        protected bool isHolding;
        protected bool isInitialized;
        public Action onGunStoppedFire;
        public Action onEmptyAmmo;
        private new void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();
        }
        private void LateUpdate() => animator.SetBool("isShooting", isShooting);
        public void FireCanceled()
        {
            isHolding = false;
            StopCoroutine(nameof(QueueShooting));
        }
        public void FirePerformed()
        {
            isHolding = true;
            StartCoroutine(nameof(QueueShooting));
        }
        private IEnumerator QueueShooting()
        {
            while (isHolding)
            {
                Fire();
                yield return new WaitForSeconds(.5f);
            }
        }
        public virtual void Init(bool enable)
        {
            gameObject.SetActive(enable);
        }
        public virtual void RefullAmmo() { }
        public virtual void ResetAmmo() { }
        public virtual void Add(int amount){}
        public virtual void AnimationEvent_StopShooting()
        {
            if (!isHolding) isShooting = false;
        }
        protected new void ShootRay()
        {
            (bool, RaycastHit) hitInfo = base.ShootRay();
            if (hitInfo.Item1)
            {
                HealthBase<float> enemyHealth = hitInfo.Item2.collider.GetComponent<HealthBase<float>>();
                enemyHealth.Take(damage);
            }
        }
        public virtual void Fire()
        {
            isShooting = true;
            animator.SetTrigger("Shoot");
        }
    }
}