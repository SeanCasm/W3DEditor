using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Game;
namespace WEditor.Game.Player
{
    public class Gun : GunBase<float>, IFullable
    {
        [SerializeField] float animSpeed = 1;
        [SerializeField] protected int maxAmmo;
        protected int currentAmmo;
        public bool hasAmmo { get => currentAmmo > 0; }
        protected Animator animator;
        protected bool isShooting;

        public bool ifFullOf => currentAmmo == maxAmmo;

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
        public void RefullAmmo() => currentAmmo = maxAmmo;
        public void Add(int amount)
        {
            if (ifFullOf)
                return;
            currentAmmo += amount;
            if (currentAmmo >= maxAmmo) currentAmmo = maxAmmo;

            GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        }
        public void Init(bool enable)
        {
            if (!isInitialized)
            {
                currentAmmo = maxAmmo;
                isInitialized = true;
            }
            gameObject.SetActive(enable);
        }
        private void LateUpdate()
        {
            animator.SetBool("isShooting", isShooting);
            animator.SetFloat("Speed", animSpeed);
        }
        private void OnEnable() => GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        private void OnDisable()
        {
            onGunStoppedFire = null;
            onEmptyAmmo = null;
            isShooting = isHolding = false;
        }

        public void Animation_Fire()
        {
            audioSource.Play();
        }
        public void FireCanceled()
        {
            isHolding = false;
        }
        public void FirePerformed()
        {
            if (!isShooting)
            {
                isHolding = true;
                Fire();
            }
        }

        public virtual void AnimationEvent_StopShooting()
        {
            isShooting = false;
            if (onGunStoppedFire != null)
                onGunStoppedFire();

            if (currentAmmo == 0)
                onEmptyAmmo();

            if (isHolding) Fire();
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