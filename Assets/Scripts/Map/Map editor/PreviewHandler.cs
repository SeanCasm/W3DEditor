using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WEditor.Events;
using WEditor.Scenario.Editor;
namespace WEditor.UI
{
    public class PreviewHandler : MonoBehaviour
    {
        public static PreviewHandler instance;
        [SerializeField] UnityEvent<bool> changeActiveState;
        [SerializeField] GameObject hudCanvas, editorPanel;
        private void Awake()
        {
            instance = this;
        }
        public void OnPreview()
        {
            if (!EditorGrid.instance.isSpawnLocated)
            {
                MessageHandler.instance.SetError("level_spawn_r");
                return;
            }
            EditorGrid.instance.InitGeneration();
            changeActiveState.Invoke(false);
            hudCanvas.SetActive(true);
            editorPanel.SetActive(false);
            EditorEvent.instance.PreviewModeEnter();
        }
        public void OnEdit()
        {
            changeActiveState.Invoke(true);
            hudCanvas.SetActive(false);
            EditorEvent.instance.PreviewModeExit();
            Time.timeScale = 1;
        }
    }
}

