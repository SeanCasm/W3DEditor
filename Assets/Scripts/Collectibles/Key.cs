using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Collectibles
{
    /// <summary>
    /// Key class for key items to open doors.
    /// </summary>
    public class Key : CollectibleBase
    {
        public KeyType keyType { get; set; }
        protected override bool OnPlayerEnter()
        {
            PlayerGlobalReference.instance.playerInventory.AddKey(keyType);
            int key = keyType == KeyType.Golden ? 0 : 1;
            GameplayEvent.instance.KeyPickedUp(key);
            return true;
        }
    }
}
public enum KeyType
{
    Golden, Platinum
}
