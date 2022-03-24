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
        [SerializeField] GameObject previewUI;
        public static bool onPreview;
        public void OnChangeHandler()
        {
            if (!EditorGrid.instance.isSpawnLocated)
            {
                TextMessageHandler.instance.SetError("pp");
                return;
            }
            onPreview = !onPreview;

            if (onPreview) OnPreview();
            else OnEdit();
        }

        private void OnPreview()
        {
            EditorGrid.instance.InitGeneration();
            changeActiveState.Invoke(false);
            previewUI.SetActive(true);
            GameEvent.instance.PreviewModeEnter();
        }
        private void OnEdit()
        {
            changeActiveState.Invoke(true);
            previewUI.SetActive(false);
            GameEvent.instance.PreviewModeExit();
        }
    }
}

