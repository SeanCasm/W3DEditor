using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Collectibles
{
    public class CollectibleBase : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer { get; set; }
        private AudioSource audioSource;
        public Sprite itemSprite;
        public int amount;
        public AudioClip collectSound;
        public Sprite GetSprite { get => SpriteRenderer.sprite; }
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        protected void OnEnable()
        {
            EditorEvent.instance.onPreviewModeExit += DestroyOnUnload;
            GameplayEvent.instance.onReset += DestroyOnUnload;
        }
        protected void OnDisable()
        {
            EditorEvent.instance.onPreviewModeExit -= DestroyOnUnload;
            GameplayEvent.instance.onReset -= DestroyOnUnload;
        }

        private void DestroyOnUnload()
        {
            Destroy(gameObject);
        }
        protected virtual bool OnPlayerEnter()
        {
            return true;
        }
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bool triggered = OnPlayerEnter();
                if (triggered)
                    OnCollected();
            }
        }
        private void OnCollected()
        {
            audioSource.PlayOneShot(collectSound);
            SpriteRenderer.enabled = false;
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(DestroyAfterSound), 1f);
        }
        private void DestroyAfterSound()
        {
            Destroy(gameObject);
        }

    }
}