using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Collectibles
{
    public class Ammo : CollectibleBase
    {
        [SerializeField] int ammoID;
        private void OnEnable()
        {
            base.OnPlayerTrigger += PlayerEnter;
        }
        private void OnDisable()
        {
            base.OnPlayerTrigger -= PlayerEnter;
        }
        private void PlayerEnter()
        {
            PlayerGlobalReference.instance.gunHandler.AddTo(ammoID, amount);
        }
    }
}
