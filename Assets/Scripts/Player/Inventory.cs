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
        public List<KeyType> keysCollected { get; private set; } = new List<KeyType>();
        private void OnEnable() => keysCollected.Add(KeyType.None);
        public void AddKey(KeyType kt) => keysCollected.Add(kt);
        private void OnDisable() => keysCollected.Clear();
    }
}
