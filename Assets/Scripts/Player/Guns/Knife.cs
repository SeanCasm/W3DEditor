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
        private void OnDisable() => StopAllCoroutines();
        private void LateUpdate() => animator.SetBool("isShooting", isShooting);
    }
}

