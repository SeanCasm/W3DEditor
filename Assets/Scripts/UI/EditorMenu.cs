using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using TMPro;
using WEditor.Scenario.Editor;
using UnityEngine.Events;

namespace WEditor.UI
{
    public class EditorMenu : MonoBehaviour
    {
        [SerializeField] TMP_InputField width, height;
        [SerializeField] UnityEvent onBack;
        public void Button_LoadLevels()
        {
            GameEvent.instance.SrollViewEnable();
        }
        public void BackFromScrollView()
        {
            GameEvent.instance.SrollViewDisable();
        }
        public void Button_BackToMainMenu()
        {
            SceneHandler.instance.LoadMain();
        }
        public void Button_BackFromCreate()
        {
            height.text = "";
            width.text = "";
        }
        public void Button_BackFromEditor()
        {
            GameEvent.instance.EditorExit();
            onBack.Invoke();
        }
    }
}
