using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Player
{
    /// <summary>
    /// Inventory class for player.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        private List<KeyType> keyType = new List<KeyType>();
        public void AddKey(KeyType kt)
        {
            keyType.Add(kt);
        }
        /// <summary>
        /// To open a door with a specific key if exists in the inventory
        /// </summary>
        /// <param name="keyToUse">The key to use.</param>
        /// <returns>if the key exists in the player inventory</returns>
        public bool UseKey(KeyType keyToUse)
        {
            return keyType.Exists(x => x == keyToUse);
        }
        private void OnDisable()
        {
            keyType.Clear();
        }
    }
}
