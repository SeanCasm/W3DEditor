using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;
using WEditor.Events;
namespace WEditor.CameraUtils
{
    public class EditorCamera : BaseCamera, IMapEditorCameraActions
    {
        new void Start()
        {
            base.Start();
            wInput.MapEditorCamera.Enable();
            wInput.MapEditorCamera.SetCallbacks(this);
        }
        private void OnEnable()
        {
            wInput?.MapEditorCamera.Enable();
        }
        private void OnDisable()
        {
            wInput.MapEditorCamera.Disable();
        }
        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                float cameraZoom = context.ReadValue<Vector2>().normalized.y;
                float fov = virtualCam.m_Lens.FieldOfView;
                if (fov >= minZoom && fov <= maxZoom)
                {
                    virtualCam.m_Lens.FieldOfView = Mathf.Clamp(fov - cameraZoom, minZoom, maxZoom);
                }
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            base.CamMove(context);
        }
        IEnumerator MoveCamera(Vector2 move)
        {
            while (move != Vector2.zero)
            {
                transform.Translate(Vector3.up * move.y * speed * Time.deltaTime);
                transform.Translate(Vector3.right * move.x * speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
