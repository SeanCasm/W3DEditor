using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;

namespace WEditor.Game.Guns
{
    public abstract class Gun : GunBase, IGunActions
    {
        [SerializeField] protected int maxAmmo;
        protected int currentAmmo;
        Animator animator;
        GunInput gunInput;
        public bool hasAmmo { get => currentAmmo > 0; }
        private void Start()
        {
            animator = GetComponent<Animator>();
            gunInput.EnableAndSetCallbacks(this);
        }
        public abstract void OnFire(InputAction.CallbackContext context);
        public void Fire()
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                ShootRay();
            }
        }
        private new void ShootRay()
        {
            Tuple<bool, RaycastHit> values = base.ShootRay();

            if (values.Item1)
            {
                HealthBase<float> enemyHealth = values.Item2.collider.GetComponent<HealthBase<float>>();
                enemyHealth.Take(damage);
            }
        }

        public void Add(int amount)
        {
            currentAmmo += amount;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        }
    }
}