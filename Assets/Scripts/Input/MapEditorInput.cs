using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;
namespace WEditor
{
    public class MapEditorInput : MonoBehaviour
    {
        public static MapEditorInput instance;
        private WInput wInput;
        private void Awake()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }
        public void EnableMapEditorCameraInputsAndSetCallbacks(IMapEditorCameraActions callbacks)
        {
            wInput.MapEditorCamera.Enable();
            wInput.MapEditorCamera.SetCallbacks(callbacks);
        }
        public void ChangeActiveMapEditorCameraInputs(bool enable)
        {
            if (enable) wInput.MapEditorCamera.Enable();
            else wInput.MapEditorCamera.Disable();
        }
        public void EnableMapPreviewInputsAndSetCallbacks(IMapPreviewActions callbacks)
        {
            wInput.MapPreview.Enable();
            wInput.MapPreview.SetCallbacks(callbacks);
        }
        public void ChangeActiveMapPreviewInputs(bool enable)
        {
            if (enable) wInput.MapPreview.Enable();
            else wInput.MapPreview.Disable();
        }
        public void EnableMapEditorInputsAndSetCallbacks(IMapEditorActions callbacks)
        {
            ChangeActiveMapEditorInputs(true);
            wInput.MapEditor.SetCallbacks(callbacks);
        }
        public void ChangeActiveMapEditorInputs(bool enable)
        {
            if (enable) wInput.MapEditor.Enable();
            else wInput.MapEditor.Disable();
        }
        public void DisableInputsForInventoryOpened()
        {
            wInput.MapEditor.Aim.Disable();
            wInput.MapEditor.Click.Disable();
        }
        public void EnableInputsForInventoryClosed()
        {
            wInput.MapEditor.Aim.Enable();
            wInput.MapEditor.Click.Enable();
        }


    }
}
