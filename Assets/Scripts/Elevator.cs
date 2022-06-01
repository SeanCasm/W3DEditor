using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Scenario.Playable
{
    /// <summary>
    /// Level ending objective
    /// </summary>
    public class Elevator : MonoBehaviour
    {
        private Animator animator;
        private bool playerInside;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Time.timeScale = 0;
                animator.SetTrigger("Up");
                // playerInside = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // playerInside = false;
            }
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

            EditorEvent.instance.PreviewModeExit();
        }
    }
}
