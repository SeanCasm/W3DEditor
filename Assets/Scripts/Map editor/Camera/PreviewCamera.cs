using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
using UnityEngine.InputSystem;
namespace WEditor.CameraUtils
{
    public class PreviewCamera : BaseCamera, IMapPreviewActions
    {
        private Rigidbody rigid;
        private new void Start()
        {
            base.Start();
            rigid = GetComponent<Rigidbody>();
            GameInput.instance.EnableMapPreviewInputsAndSetCallbacks(this);
        }
        private void OnEnable()
        {
            GameInput.instance.ChangeActiveMapPreviewInputs(true);
        }
        private void OnDisable()
        {
            GameInput.instance.ChangeActiveMapPreviewInputs(false);
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
                transform.Rotate(Vector3.up, axis, Space.World);
                yield return null;
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
                rigid.velocity = Vector3.zero;
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
