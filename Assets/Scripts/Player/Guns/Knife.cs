using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Knife : Gun
    {
        public override void Fire()
        {
            base.Fire();
            base.ShootRay();
        }
        private void OnEnable() => GameplayEvent.instance.AmmoChanged("-");
        private void LateUpdate() => animator.SetBool("isShooting", isShooting);
        public override void AnimationEvent_StopShooting()
        {
            isShooting = false;
            if (onGunStoppedFire != null)
                onGunStoppedFire();

            if (isHolding) Fire();
        }
    }
}

