using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using WEditor.Events;
using WEditor.Scenario;
namespace WEditor.UI
{
    public class PreviewHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent<bool> changeActiveState;
        [SerializeField] GameObject previewUI;
        private TextMeshProUGUI buttonText;
        private bool onPreview = false;
        private void Awake()
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }
        public void OnChangeHandler()
        {
            if (!EditorGrid.instance.isSpawnLocated)
            {
                TextMessageHandler.instance.PP();
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
            buttonText.text = "Exit";
        }
        private void OnEdit()
        {
            buttonText.text = "Play";
            changeActiveState.Invoke(true);
            previewUI.SetActive(false);
            GameEvent.instance.PreviewModeExit();
        }
    }
}

