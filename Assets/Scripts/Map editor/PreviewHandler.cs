using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using WEditor.Events;
namespace WEditor.UI
{
    public class PreviewHandler : MonoBehaviour
    {
        [SerializeField] List<GameObject> objectsToDisable;
        private TextMeshProUGUI buttonText;
        private bool onPreview = false;
        private void Awake()
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        }
        private void OnDisable()
        {

        }
        public void OnChangeHandler()
        {
            onPreview = !onPreview;

            if (onPreview) OnPreview();
            else OnEdit();
        }

        private void OnPreview()
        {
            objectsToDisable.ForEach(item =>
            {
                item.SetActive(false);
            });
            GameEvent.instance.PreviewModeEnter();
            buttonText.text = "Exit";
        }
        private void OnEdit()
        {
            buttonText.text = "Preview";
            objectsToDisable.ForEach(item =>
            {
                item.SetActive(true);
            });
            GameEvent.instance.PreviewModeExit();
        }
    }
}

