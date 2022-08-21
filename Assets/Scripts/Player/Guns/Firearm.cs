using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Firearm : Gun
    {
        public override void Fire()
        {
            GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
            base.ShootRay();

            currentAmmo -= 1;
            base.Fire();
            if (currentAmmo == 0)
            {
                onEmptyAmmo();
                return;
            }
        }
    }
}