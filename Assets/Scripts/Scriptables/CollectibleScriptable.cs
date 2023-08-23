using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Collectibles;

namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "ScriptableObjects/GlobalCollectible")]
    public class CollectibleScriptable : ScriptableObject
    {
        public Sprite itemSprite;
        public string spriteName { get => itemSprite.name; }
        public int amount;
        public AudioClip collectSound;
        public void LoadFromScriptable(CollectibleBase collBase)
        {
            collBase.amount = this.amount;
            collBase.collectSound = this.collectSound;
            collBase.SpriteRenderer ??= collBase.GetComponent<SpriteRenderer>();
            collBase.SpriteRenderer.sprite = this.itemSprite;
        }
    }
}