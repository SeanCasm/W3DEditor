using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WEditor.Events;
using static WInput;
namespace WEditor.Game.Player
{

    public class PlayerController : MonoBehaviour, IPlayerActions
    {
        [Header("Movement")]
        [SerializeField] float speed, sprintSpeed;
        [Range(0, 5)]
        [SerializeField] float rotationSpeed;
        public static float currentRotationSpeed;
        private Rigidbody rigid;
        private GunHandler gunHandler;
        private bool isMovingMouse, isMoving;
        private Vector2 movement = Vector2.zero;
        private float currentSpeed;
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerControllerInput.instance.Disable();
            StopAllCoroutines();
        }
        private void OnEnable()
        {
            currentRotationSpeed = PlayerPrefs.HasKey("aim") ? PlayerPrefs.GetFloat("aim") : rotationSpeed;
            Cursor.lockState = CursorLockMode.Locked;
            rigid = GetComponent<Rigidbody>();
            gunHandler = GetComponentInChildren<GunHandler>();
            PlayerControllerInput.instance.EnableAndSetCallbacks(this);
            currentSpeed = speed;
        }
        void FixedUpdate()
        {
            if (isMoving)
            {
                Vector3 dir = transform.forward * movement.y + transform.right * movement.x;
                rigid.MovePosition(rigid.position + dir * currentSpeed * Time.deltaTime);
            }
            else
            {
                rigid.velocity = Vector3.zero;
            }
        }
        public void OnMovement(InputAction.CallbackContext context)
        {
            movement = context.ReadValue<Vector2>();
            if (context.started)
            {
                isMoving = true;
                currentSpeed = speed;
            }
            else if (context.canceled)
            {
                isMoving = false;
                currentSpeed = 0;
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
                transform.Rotate(Vector3.up, axis * currentRotationSpeed, Space.World);
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
                gunHandler.SwapToGunWithAmmo();
            }
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameplayEvent.instance.Interacted(PlayerGlobalReference.instance.PlayerInventory.keysCollected);
            }
        }
    }
}
