using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using TMPro;
namespace WEditor.UI
{
    public class EditorMenu : MonoBehaviour
    {
        [SerializeField] TMP_InputField width, height;
        public void Button_LoadMapEditorFromCreate()
        {
            int w = int.Parse(width.text);
            int h = int.Parse(height.text);
            if (w < 8 || h < 8)
            {
                MessageHandler.instance.SetError("level_size_l");
                return;
            }
            if (w > 50 || h > 50)
            {
                MessageHandler.instance.SetError("level_size_u");
                return;
            }
            SceneHandler.instance.Button_LoadEditorFromCreateOption(w, h);
        }
        public void Button_Settings()
        {
            GameSettingsMenu.instance.Button_Enable();
        }
        public void Button_LoadLevels()
        {
            EditorEvent.instance.SrollViewEnable();
        }
        public void BackFromScrollView()
        {
            EditorEvent.instance.SrollViewDisable();
        }
        public void Button_BackToMainMenu()
        {
            SceneHandler.instance.LoadMain();
        }
        public void Button_BackToPreEditor()
        {
            EditorEvent.instance.EditorExit();
            SceneHandler.instance.LoadPreMapEditor();
        }
        public void Button_BackFromCreate()
        {
            height.text = "";
            width.text = "";
        }
        public void Button_BackFromEditor()
        {
            EditorEvent.instance.EditorExit();
            SceneHandler.instance.LoadPreMapEditor();
        }
    }
}
