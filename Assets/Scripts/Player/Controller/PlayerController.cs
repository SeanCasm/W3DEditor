using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;
using WEditor.Game.Guns;
namespace WEditor.Game.Player
{

    public class PlayerController : MonoBehaviour, IPlayerActions
    {
        [SerializeField] float speed, sprintSpeed;
        private Rigidbody rigid;
        private float currentSpeed;
        PlayerControllerInput playerControllerInput;
        GunHandler gunHandler;
        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
            gunHandler = GetComponentInChildren<GunHandler>();
            playerControllerInput = new PlayerControllerInput();
            playerControllerInput.EnableAndSetCallbacks(this);
            currentSpeed = speed;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 move = context.ReadValue<Vector2>();
            if (context.started)
            {
                StartCoroutine(nameof(Move), move);
            }
            else if (context.canceled)
            {
                rigid.velocity = Vector3.zero;
                StopCoroutine(nameof(Move));
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

        public void OnRotatemouse(InputAction.CallbackContext context)
        {
            Vector2 mousePosition = context.ReadValue<Vector2>();
            // if (context.started)
            // {
            //     StartCoroutine(nameof(RotateAround), mousePosition.x);
            // }
            // else if (context.canceled)
            // {
            //     StopCoroutine(nameof(RotateAround));
            // }
        }

        IEnumerator RotateAround(float axis)
        {
            while (axis != 0)
            {
                transform.Rotate(Vector3.up, axis, Space.World);
                yield return null;
            }
        }
        IEnumerator Move(Vector2 move)
        {
            while (move != Vector2.zero)
            {
                Vector3 dir = transform.forward * move.y + transform.right * move.x;
                rigid.MovePosition(rigid.position + dir * currentSpeed * Time.deltaTime);
                yield return null;
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentSpeed = sprintSpeed;
            }
            else if (context.canceled)
            {
                currentSpeed = speed;
            }
        }

        public void OnSwapgun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                gunHandler.TrySwapGun();
            }
        }
    }
}
