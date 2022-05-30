using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Collectibles
{
    public class Health : CollectibleBase
    {
        private void OnEnable()
        {
            base.OnPlayerTrigger += OnPlayerEnter;
        }
        private void OnDisable()
        {
            base.OnPlayerTrigger -= OnPlayerEnter;
        }
        private bool OnPlayerEnter()
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
