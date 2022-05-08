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
        [SerializeField] GameObject previewUI,editorPanel;
        public void OnPreview()
        {
            if (!EditorGrid.instance.isSpawnLocated)
            {
                TextMessageHandler.instance.SetError("pp");
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
        }
    }
}

