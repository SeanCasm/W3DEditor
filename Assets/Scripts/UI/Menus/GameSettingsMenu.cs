using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.UI
{
    /// <summary>
    /// This class handles the game settings menu UI.
    /// </summary>
    public class GameSettingsMenu : MonoBehaviour
    {
        public static GameSettingsMenu instance;
        [SerializeField] GameObject settingMenu;
        [SerializeField] GameObject gameplayScreen, editorScreen, audioScreen;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        public void Button_Enable()
        {
            MenuManager.instance.currentMenu = CurrentMenu.Setting;
            settingMenu.SetActive(true);
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
        public void Button_Back()
        {
            MenuManager.instance.currentMenu = CurrentMenu.None;
            settingMenu.SetActive(false);
        }
    }

}