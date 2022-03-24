using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Scenario;
using WEditor.Events;
namespace WEditor.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject mainMenu, levelSelectorMenu;
        public void Button_PlayLevels()
        {
            GameEvent.instance.SrollViewEnable();
            mainMenu.SetActive(false);
            levelSelectorMenu.SetActive(true);
        }
        public void BackFromScrollView()
        {
            mainMenu.SetActive(true);
            levelSelectorMenu.SetActive(false);
            GameEvent.instance.SrollViewDisable();
        }
    }
}