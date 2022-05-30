using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "ScriptableObjects/GlobalCollectible")]
    public class CollectibleScriptable : ScriptableObject
    {
        public Sprite itemSprite;
        public string spriteName { get => itemSprite.name; }
        public int amount;
        public AudioClip collectSound;
    }
}