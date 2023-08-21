using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Game.Scriptables;
namespace WEditor.Game.Collectibles
{
    public class CollectibleBase : MonoBehaviour
    {
        /// <summary>
        /// Boolean defines if the current action happens without problems
        /// </summary>
        protected event System.Func<bool> OnPlayerTrigger;
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
        public Sprite getSprite { get => spriteRenderer.sprite; }
        private void OnEnable()
        {
            OnPlayerTrigger += OnPlayerEnter;
            EditorEvent.instance.onPreviewModeExit += DestroyOnUnload;
        }
        private void OnDisable()
        {
            OnPlayerTrigger -= OnPlayerEnter;
            EditorEvent.instance.onPreviewModeExit -= DestroyOnUnload;
        }

        private void DestroyOnUnload()
        {
            Destroy(gameObject);
        }
        private void Update()
        {
            transform.LookAt(PlayerGlobalReference.instance.position, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        protected virtual bool OnPlayerEnter()
        {
            return true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (OnPlayerTrigger != null && other.CompareTag("Player"))
            {
                bool triggered = OnPlayerTrigger();
                if (triggered)
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