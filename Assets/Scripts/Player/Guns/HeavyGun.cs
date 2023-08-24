using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{

    public class HeavyGun : Gun, IFullable
    {
        [SerializeField] int bulletBurst;
        private bool coroutineOn;
        public override void Fire()
        {
            base.Fire();
            if (!coroutineOn)
                StartCoroutine(nameof(BurstShooting));
        }
        public override void AnimationEvent_StopShooting()
        {
            isShooting = false;

            if (currentAmmo == 0)
                onEmptyAmmo();

            if (isHolding) base.Fire();
        }
        IEnumerator BurstShooting()
        {
            coroutineOn = true;
            int bb = 0;
            while (bb != bulletBurst && isHolding)
            {
                currentAmmo--;
                GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
                base.ShootRay();
                bb++;
                if (currentAmmo == 0)
                {
                    onEmptyAmmo();
                    coroutineOn = false;
                    yield break;
                }
                yield return new WaitForSeconds(0.15f);
                bb = 0;
            }
            coroutineOn = false;
        }
    }
}
