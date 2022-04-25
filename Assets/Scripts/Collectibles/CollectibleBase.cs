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
        private CollectibleScriptable collectibleScriptable;
        public CollectibleScriptable CollectibleScriptable
        {
            get
            {
                return collectibleScriptable;
            }
            set
            {
                collectibleScriptable = value;
                spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = collectibleScriptable.itemSprite;
                amount = collectibleScriptable.amount;
            }
        }
        private void Update()
        {
            transform.LookAt(PlayerGlobalReference.instance.position, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
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
            if (CollectibleScriptable.collectSound != null)
                AudioSource.PlayClipAtPoint(CollectibleScriptable.collectSound, transform.position);

            Destroy(gameObject);
        }
    }
}