using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
namespace WEditor.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject mainMenu, levelSelectorMenu;
        
        public void Button_PlayLevels()
        {
            EditorEvent.instance.SrollViewEnable();
            mainMenu.SetActive(false);
            levelSelectorMenu.SetActive(true);
        }
        public void Button_Settings()
        {
            GameSettingsMenu.instance.Button_Enable();
        }
        public void Button_LoadPreEditor()
        {
            SceneHandler.instance.LoadPreMapEditor();
        }
        public void BackFromScrollView()
        {
            mainMenu.SetActive(true);
            levelSelectorMenu.SetActive(false);
            EditorEvent.instance.SrollViewDisable();
        }
        public void Button_Exit()
        {
            Application.Quit();
        }
    }
}