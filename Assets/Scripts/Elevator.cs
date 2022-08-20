using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Game.Scriptables;

namespace WEditor.Scenario.Playable
{
    /// <summary>
    /// Level ending objective
    /// </summary>
    public class Elevator : MonoBehaviour, IInteractable
    {
        public KeyDoorScriptable keyDoorScriptable { get; set; }
        private Animator animator;
        private bool playerAround = false;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            GameplayEvent.instance.onInteracted += OnInteracted;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onInteracted -= OnInteracted;
        }
        public void OnInteracted(List<KeyType> keyToOpen)
        {
            if (playerAround)
            {
                animator.SetTrigger("Up");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                playerAround = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                playerAround = false;
        }
        /// <summary>
        /// Called in animation event
        /// </summary>
        public void EndGame()
        {
            if (!SceneHandler.instance.isEditorScene)
            {
                SceneHandler.instance.LoadEndGameScene();
                return;
            }
            WEditor.UI.PreviewHandler.instance.OnEdit();
        }
    }
}
