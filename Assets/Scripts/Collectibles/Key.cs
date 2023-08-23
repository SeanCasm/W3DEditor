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
        public KeyType KeyType { get; private set; }
        public void SetKeyTypeFromName(string n)
        {
            KeyType = n.Contains("Golden") ? KeyType.Golden : KeyType.Platinum;
        }
        protected override bool OnPlayerEnter()
        {
            PlayerGlobalReference.instance.playerInventory.AddKey(KeyType);
            int key = KeyType == KeyType.Golden ? 0 : 1;
            GameplayEvent.instance.KeyPickedUp(key);
            return true;
        }
    }
}
public enum KeyType
{
    Golden, Platinum, None
}
