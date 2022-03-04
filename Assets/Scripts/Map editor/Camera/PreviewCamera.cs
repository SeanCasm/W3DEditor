using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
using UnityEngine.InputSystem;
using WEditor.Events;
namespace WEditor.CameraUtils
{
    public class PreviewCamera : BaseCamera, IMapPreviewActions
    {
        private new void Start()
        {
            base.Start();
            wInput.MapPreview.Enable();
            wInput.MapPreview.SetCallbacks(this);
        }
        private void OnEnable() {
            wInput?.MapPreview.Enable();
        }
        private void OnDisable() {
            wInput.MapPreview.Disable();
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
        IEnumerator RotateAround(float axis)
        {
            while (axis != 0)
            {
                transform.Rotate(-Vector3.forward, axis, Space.World);
                yield return null;
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
