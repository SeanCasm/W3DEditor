using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Firearm : Gun, IFullable
    {
        [SerializeField] protected int maxAmmo;
        [SerializeField] int bulletBurst = 1;
        private int currentAmmo;
        public bool hasAmmo { get => currentAmmo > 0; }
        public bool ifFullOf => currentAmmo == maxAmmo;

        private void OnEnable() => GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        private void OnDisable()
        {
            StopAllCoroutines();
            onGunStoppedFire = null;
            onEmptyAmmo = null;
        }
        public override void RefullAmmo() => currentAmmo = maxAmmo;
        public override void ResetAmmo() => currentAmmo = 0;

        public override void Init(bool enable)
        {
            if (!isInitialized)
            {
                currentAmmo = maxAmmo;
                isInitialized = true;
            }
            base.Init(enable);
        }

        public override void Fire()
        {
            if (bulletBurst > 1)
            {
                StartCoroutine(nameof(BurstShooting));
            }
            else
            {
                currentAmmo -= bulletBurst;
                GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
                ShootRay();
            }
            base.Fire();
            if (currentAmmo == 0)
            {
                onEmptyAmmo();
                return;
            }
        }
        IEnumerator BurstShooting()
        {
            int bb = 0;
            while (bb != bulletBurst)
            {
                currentAmmo--;
                yield return new WaitForSeconds(0.15f);
                GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
                ShootRay();
                bb++;
            }
        }
        public override void AnimationEvent_StopShooting()
        {
            base.AnimationEvent_StopShooting();

            if (onGunStoppedFire != null)
                onGunStoppedFire();
        }
        public override void Add(int amount)
        {
            currentAmmo += amount;
            if (currentAmmo >= maxAmmo)
                currentAmmo = maxAmmo;
        }
    }
}