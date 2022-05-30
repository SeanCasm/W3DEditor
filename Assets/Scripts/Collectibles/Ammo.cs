using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;

namespace WEditor.Game.Collectibles
{
    public class Ammo : CollectibleBase
    {
        public int ammoID { get; set; }
        public static int ammoDevalue = 0;
        private void OnEnable()
        {
            base.OnPlayerTrigger += PlayerEnter;
        }
        private void OnDisable()
        {
            base.OnPlayerTrigger -= PlayerEnter;
        }
        private bool PlayerEnter()
        {
            GunHandler gunHandler = PlayerGlobalReference.instance.gunHandler;
            if (gunHandler.currentGun is Firearm && !(gunHandler.currentGun as Firearm).ifFullOf)
            {
                gunHandler.AddTo(amount * (ammoDevalue / 100));
                return true;
            }
            return false;
        }
    }
}
