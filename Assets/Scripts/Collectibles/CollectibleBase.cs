using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Scriptables;
namespace WEditor.Game.Collectibles
{
    public class CollectibleBase : MonoBehaviour
    {
        protected event System.Action OnPlayerTrigger;
        protected int amount;
        private SpriteRenderer spriteRenderer;
        public CollectibleScriptable collectibleScriptable { get; set; }
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = collectibleScriptable.itemSprite;
            amount = collectibleScriptable.amount;
        }
        private void Update()
        {
            transform.LookAt(PlayerGlobalReference.instance.position);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (OnPlayerTrigger != null && other.CompareTag("Player"))
            {
                OnPlayerTrigger();
                OnCollected();
            }
        }
        private void OnCollected()
        {
            if (collectibleScriptable.collectSound != null)
                AudioSource.PlayClipAtPoint(collectibleScriptable.collectSound, transform.position);

            Destroy(gameObject);
        }
    }
}