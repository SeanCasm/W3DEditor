using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;
using WEditor.Input;

namespace WEditor.CameraUtils
{
    public class EditorCamera : BaseCamera, IMapEditorCameraActions
    {
        new void Start()
        {
            base.Start();
            EditorCameraInput.instance.EnableAndSetCallbacks(this);
            EditorCameraInput.instance.SetRotateInput(false);
        }
        private void OnEnable()
        {
            EditorCameraInput.instance.Enable();
        }
        private void OnDisable()
        {
            EditorCameraInput.instance.Disable();
        }
        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                float cameraZoom = context.ReadValue<Vector2>().normalized.y;
                Vector3 currPos = transform.position;
                float y = currPos.y;

                float yClamped = Mathf.Clamp(y - cameraZoom, maxZoom, minZoom);
                transform.position = new Vector3(currPos.x, yClamped, currPos.z);
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
                transform.Translate(Vector3.forward * move.y * currentSpeed * Time.deltaTime);
                transform.Translate(Vector3.right * move.x * currentSpeed * Time.deltaTime);
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
