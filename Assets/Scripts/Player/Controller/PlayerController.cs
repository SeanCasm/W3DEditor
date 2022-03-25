using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;
using WEditor.Game.Player.Guns;
using WEditor.Events;
namespace WEditor.Game.Player
{

    public class PlayerController : MonoBehaviour, IPlayerActions
    {
        [Header("Movement")]
        [SerializeField] float speed, sprintSpeed;
        [SerializeField] float rotationSpeed;
        private Rigidbody rigid;
        private GunHandler gunHandler;
        private bool isMovingMouse;
        private float currentSpeed;
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
            gunHandler = GetComponentInChildren<GunHandler>();
            PlayerControllerInput.instance.EnableAndSetCallbacks(this);
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
            float mousePosition = context.ReadValue<Vector2>().x;
            mousePosition = mousePosition > 0 ? 1 : -1;
            if (context.performed)
            {
                if (!isMovingMouse) StartCoroutine(nameof(RotateAround), mousePosition);
                isMovingMouse = true;
            }
            else if (context.canceled)
            {
                isMovingMouse = false;
                StopCoroutine(nameof(RotateAround));
            }
        }

        IEnumerator RotateAround(float axis)
        {
            while (axis != 0)
            {
                transform.Rotate(Vector3.up, axis * rotationSpeed, Space.World);
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
