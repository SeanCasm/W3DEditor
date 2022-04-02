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
        private void OnPlayerEnter()
        {
            PlayerGlobalReference.instance.playerHealth.Add(amount);
        }
    }
}
