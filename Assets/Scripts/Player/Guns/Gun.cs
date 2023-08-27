using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
namespace WEditor.Game.Player
{
    public class Gun : GunBase<float>, IFullable
    {
        [SerializeField] float animSpeed = 1;
        [SerializeField] protected int maxAmmo;
        [SerializeField] int inventoryIndex;
        protected int currentAmmo;
        public int InventoryIndex => inventoryIndex;
        public bool HasAmmo { get => currentAmmo > 0; }
        protected Animator animator;
        public bool IsShooting { get; protected set; }
        public bool isFullOfAmmo => currentAmmo == maxAmmo;

        protected bool isHolding;
        public Action onEmptyAmmo;
        private new void Start()
        {
            base.Start();
            shootPoint = transform.GetChild(0);
            animator = GetComponent<Animator>();
        }
        public void SwapToThisFromGround()
        {
            Enable();
            RefullAmmo();
            GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        }
        public void RefullAmmo() => currentAmmo = maxAmmo;
        public void Add(int amount)
        {
            if (isFullOfAmmo || !gameObject.activeSelf)
                return;
            currentAmmo += amount;
            if (currentAmmo >= maxAmmo) currentAmmo = maxAmmo;

            GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }
        public void Reset()
        {
            currentAmmo = 0;
            onEmptyAmmo = null;
            Disable();
        }
        public void Enable()
        {
            gameObject.SetActive(true);
        }
        private void LateUpdate()
        {
            animator.SetBool("isShooting", IsShooting);
            animator.SetFloat("Speed", animSpeed);
        }
        private void OnEnable() => GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        private void OnDisable()
        {
            IsShooting = isHolding = false;
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
            if (!IsShooting)
            {
                isHolding = true;
                Fire();
            }
        }

        public virtual void AnimationEvent_StopShooting()
        {
            IsShooting = false;
            currentAmmo -= 1;
            if (currentAmmo <= 0)
            {
                onEmptyAmmo();
                return;
            }
            if (isHolding) Fire();
        }
        public void ShootRay()
        {
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, checkDistance, hitLayer))
            {
                Enemy.Health healthComponent = hit.collider.GetComponent<Enemy.Health>();

                healthComponent?.Take(damage);
#if UNITY_EDITOR
                Debug.DrawLine(shootPoint.position, hit.point, Color.green, 5);
#endif
            }

        }
        public virtual void Fire()
        {
            IsShooting = true;
            animator.SetTrigger("Shoot");
        }
    }
}