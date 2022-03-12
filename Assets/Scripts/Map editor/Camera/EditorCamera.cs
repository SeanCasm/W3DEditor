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
            GameInput.instance.EnableMapEditorCameraInputsAndSetCallbacks(this);
        }
        private void OnEnable()
        {
            GameInput.instance.ChangeActiveMapEditorCameraInputs(true);
        }
        private void OnDisable()
        {
            GameInput.instance.ChangeActiveMapEditorCameraInputs(false);
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
            Vector2 move = context.ReadValue<Vector2>();
            if (context.started)
            {
                StartCoroutine("MoveCamera", move);
            }
            else if (context.canceled)
            {
                StopCoroutine("MoveCamera");
            }
        }
        IEnumerator MoveCamera(Vector2 move)
        {
            while (move != Vector2.zero)
            {
                transform.Translate(Vector3.forward * move.y * speed * Time.deltaTime);
                transform.Translate(Vector3.right * move.x * speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
