using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using static WInput;
namespace WEditor.Input
{
    public class EditorCameraInput : MonoBehaviour
    {
        public static EditorCameraInput instance;
        private WInput wInput;
        private void OnEnable()
        {
            GameEvent.instance.onPreviewModeEnter += OnPreviewMode;
            GameEvent.instance.onPreviewModeExit += OnEditorMode;
        }
        private void OnDisable()
        {
            GameEvent.instance.onPreviewModeEnter -= OnPreviewMode;
            GameEvent.instance.onPreviewModeExit -= OnEditorMode;
        }
        private void Start()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }
        public void SetRotateInput(bool enable)
        {
            if (enable) wInput.MapEditorCamera.Rotate.Enable();
            else wInput.MapEditorCamera.Rotate.Disable();
        }
        public void EnableAndSetCallbacks(IMapEditorCameraActions callbacks)
        {
            wInput.MapEditorCamera.Enable();
            wInput.MapEditorCamera.SetCallbacks(callbacks);
        }
        private void OnEditorMode()
        {
            wInput.MapEditorCamera.Rotate.Disable();
        }
        private void OnPreviewMode()
        {
            wInput.MapEditorCamera.Rotate.Enable();
        }
        public void Enable()
        {
            wInput.MapEditorCamera.Enable();
            wInput.MapEditorCamera.Rotate.Disable();
        }
        public void Disable()
        {
            wInput.MapEditorCamera.Disable();
        }
    }
}
