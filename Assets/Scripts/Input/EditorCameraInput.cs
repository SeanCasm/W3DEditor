using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
namespace WEditor.Input
{
    public class EditorCameraInput : MonoBehaviour
    {
        public static EditorCameraInput instance;
        private WInput wInput;
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
        public void ChangeActiveCameraInputs(bool enable)
        {
            if (enable)
            {
                wInput.MapEditorCamera.Enable();
                wInput.MapEditorCamera.Rotate.Enable();
            }
            else
            {
                wInput.MapEditorCamera.Disable();
                wInput.MapEditorCamera.Rotate.Disable();
            }
        }

    }
}
