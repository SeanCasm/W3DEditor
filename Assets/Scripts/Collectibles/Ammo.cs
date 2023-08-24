using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;

namespace WEditor.Game.Collectibles
{
    public class Ammo : CollectibleBase
    {
        public static int ammoDevalue = 0;
        protected override bool OnPlayerEnter()
        {
            GunHandler gunHandler = PlayerGlobalReference.instance.GunHandler;
            if (gunHandler.currentGun is Firearm && !(gunHandler.currentGun as Firearm).isFullOfAmmo)
            {
                gunHandler.AddTo(amount - ammoDevalue);
                return true;
            }
            return false;
        }
    }
}
