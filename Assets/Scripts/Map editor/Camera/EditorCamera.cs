using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;
using WEditor.Events;
using WEditor.Input;
namespace WEditor.CameraUtils
{
    public class EditorCamera : BaseCamera, IMapEditorCameraActions
    {
        new void Start()
        {
            base.Start();
            EditorCameraInput.instance.SetRotateInput(false);
            EditorCameraInput.instance.EnableAndSetCallbacks(this);
        }
        private void OnEnable()
        {
            EditorCameraInput.instance.ChangeActiveCameraInputs(true);
        }
        private void OnDisable()
        {
            EditorCameraInput.instance.ChangeActiveCameraInputs(false);
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
        IEnumerator RotateAround(float axis)
        {
            while (axis != 0)
            {
                transform.Rotate(Vector3.up, axis, Space.World);
                yield return null;
            }
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            float axis = context.ReadValue<float>();

            if (context.started)
            {
                StartCoroutine(nameof(RotateAround), axis);
            }
            else if (context.canceled)
            {
                StopCoroutine(nameof(RotateAround));
            }
        }
    }
}
