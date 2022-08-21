using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.UI
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        public CurrentMenu currentMenu { get; set; } = CurrentMenu.None;
        private void Awake()
        {
            instance = this;
        }
    }
    public enum CurrentMenu
    {
        Pause, Setting, None
    }
}
