using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using WEditor.Events;
using WEditor.Scenario.Editor;
namespace WEditor.UI
{
    public class PreviewHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent<bool> changeActiveState;
        [SerializeField] GameObject previewUI, editorPanel;
        public void OnPreview()
        {
            if (!EditorGrid.instance.isSpawnLocated)
            {
                MessageHandler.instance.SetError("level_spawn_r");
                return;
            }
            EditorGrid.instance.InitGeneration();
            changeActiveState.Invoke(false);
            previewUI.SetActive(true);
            editorPanel.SetActive(false);
            EditorEvent.instance.PreviewModeEnter();
        }
        public void OnEdit()
        {
            changeActiveState.Invoke(true);
            previewUI.SetActive(false);
            EditorEvent.instance.PreviewModeExit();
            Time.timeScale = 1;
        }
    }
}

