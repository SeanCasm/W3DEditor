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
            base.ShootRay();
            base.Fire();
            GameplayEvent.instance.AmmoChanged(currentAmmo.ToString());
        }

    }
}