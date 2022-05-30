using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Collectibles
{
    public class Health : CollectibleBase
    {
        
        protected override bool OnPlayerEnter()
        {
            WEditor.Game.Player.Health pHealth = PlayerGlobalReference.instance.playerHealth;
            if (!pHealth.ifFullOf)
            {
                pHealth.Add(amount);
                return true;
            }
            return false;
        }
    }
}
