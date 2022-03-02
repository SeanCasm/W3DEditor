using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
using UnityEngine.InputSystem;

namespace WEditor.CameraUtils
{
    public class PreviewCamera : BaseCamera, IMapPreviewActions
    {
        private new void Start()
        {
            base.Start();
            wInput.MapPreview.SetCallbacks(this);
        }
        private new void OnEnable()
        {
            base.OnEnable();
            wInput.MapPreview.Enable();
        }
        private void OnDisable()
        {
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
                virtualCam.transform.RotateAround(pointViewCenter, Vector3.up, 30 * axis * Time.deltaTime);
                yield return null;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }


}
