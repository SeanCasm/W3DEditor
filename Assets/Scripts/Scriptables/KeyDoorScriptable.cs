using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "KeyDoor", menuName = "ScriptableObjects/KeyDoor")]
    public class KeyDoorScriptable : ScriptableObject
    {
        public KeyType keyType;
        public Sprite doorSprite;
        public float timeToClose;
    }
}