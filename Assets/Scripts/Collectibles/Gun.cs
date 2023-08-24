using System.Collections;
using System.Collections.Generic;
namespace WEditor.Game.Collectibles
{
    public class Gun : CollectibleBase
    {
        public int gunIndex { get; set; }
        protected override bool OnPlayerEnter()
        {
            PlayerGlobalReference.instance.GunHandler.AddGun(gunIndex);
            return true;
        }
    }
}
