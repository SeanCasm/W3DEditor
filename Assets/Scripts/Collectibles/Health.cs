using System.Collections;
using System.Collections.Generic;
namespace WEditor.Game.Collectibles
{
    public class Health : CollectibleBase
    {

        protected override bool OnPlayerEnter()
        {
            WEditor.Game.Player.Health pHealth = PlayerGlobalReference.instance.PlayerHealth;
            if (!pHealth.isFullOfAmmo)
            {
                pHealth.Add(amount);
                return true;
            }
            return false;
        }
    }
}
