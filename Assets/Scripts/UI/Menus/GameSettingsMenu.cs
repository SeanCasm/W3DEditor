using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WEditor.UI
{
    /// <summary>
    /// This class handle the game settings menu UI instantiation.
    /// </summary>
    public class GameSettingsMenu : MonoBehaviour
    {
        public static GameSettingsMenu instance;
        [SerializeField] Transform currentMainCanvas;
        [SerializeField] GameObject settingsMenuPrefab;
        [SerializeField] UnityEvent<bool> enableSomething;
        private Transform gameplayScreen, editorScreen, audioScreen;
        private Button backButton;
        private GameObject settingsSceneReference;
        private void Start() => instance = this;
        
        /// <summary>
        /// Instantiate the settings prefab into the current canvas in scene.
        /// </summary>
        public void InstantiateSettingsMenu()
        {
            enableSomething.Invoke(false);
            settingsSceneReference =
            Instantiate(
                settingsMenuPrefab,
                currentMainCanvas.transform.position,
                Quaternion.identity,
                currentMainCanvas
            );
            //Find the back button to add a event listener to destroy the settings menu itself.
            backButton = ToFind("Back").GetComponent<Button>();
            backButton.onClick.AddListener(() => DestroySelf());

            gameplayScreen = ToFind("Gameplay screen");

            audioScreen = ToFind("Audio screen");

            editorScreen = ToFind("Editor screen");

            Transform ToFind(string toFind)
            {
                return settingsSceneReference.transform.Find(toFind);
            }
        }
        public void ChangeToGameplayScreen()
        {
            gameplayScreen.gameObject.SetActive(true);
            audioScreen.gameObject.SetActive(false);
            editorScreen.gameObject.SetActive(false);
        }
        public void ChangeToAudioScreen()
        {
            gameplayScreen.gameObject.SetActive(false);
            audioScreen.gameObject.SetActive(true);
            editorScreen.gameObject.SetActive(false);
        }
        public void ChangeToEditorScreen()
        {
            gameplayScreen.gameObject.SetActive(false);
            audioScreen.gameObject.SetActive(false);
            editorScreen.gameObject.SetActive(true);
        }
        /// <summary>
        /// Destroy the settings gameobject reference instantiated in the scene.
        /// </summary>
        public void DestroySelf()
        {
            enableSomething.Invoke(true);
            Destroy(settingsSceneReference);
        }
    }
}