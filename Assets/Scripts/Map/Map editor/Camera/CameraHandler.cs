using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.CameraUtils
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] EditorCamera editorCamera;
        private void OnEnable()
        {
            EditorEvent.instance.onPreviewModeEnter += OnPreviewModeEnter;
            EditorEvent.instance.onPreviewModeExit += OnPreviewModeExit;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onPreviewModeEnter -= OnPreviewModeEnter;
            EditorEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
        }
        private void OnPreviewModeEnter()
        {
            editorCamera.gameObject.SetActive(false);
        }
        private void OnPreviewModeExit()
        {
            editorCamera.gameObject.SetActive(true);
        }
    }
}