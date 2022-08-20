using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
namespace WEditor.Game.Player
{
    public class Gun : GunBase<float>
    {
        [SerializeField] float animSpeed=1;
        protected Animator animator;
        public bool isShooting { get; private set; }
        protected bool isHolding;
        protected bool isInitialized;
        public Action onGunStoppedFire;
        public Action onEmptyAmmo;
        private new void Start()
        {
            base.Start();
            shootPoint = transform.GetChild(0);
            animator = GetComponent<Animator>();
        }
        private void LateUpdate()
        {
            animator.SetBool("isShooting", isShooting);
            animator.SetFloat("Speed", animSpeed);
        }
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
        public virtual void Add(int amount) { }
        public virtual void AnimationEvent_StopShooting()
        {
            if (!isHolding) isShooting = false;
        }
        public void ShootRay()
        {
            RaycastHit[] raycastHit = Physics.RaycastAll(shootPoint.position, shootPoint.forward,
            checkDistance, hitLayer);
            if (raycastHit.Length > 0)
            {
                Enemy.Health healthComponent = raycastHit[0].collider.GetComponent<Enemy.Health>();
                healthComponent.Take(damage);
            }

            Debug.DrawLine(shootPoint.position, shootPoint.forward * checkDistance, Color.green, 5);

        }
        public virtual void Fire()
        {
            isShooting = true;
            animator.SetTrigger("Shoot");
        }
    }
}