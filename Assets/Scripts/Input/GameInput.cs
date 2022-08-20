using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
namespace WEditor
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput instance;
        public WInput wInput { get; private set; }
        private void OnEnable()
        {
            GameplayEvent.instance.onDeath += OnDeath;
            GameplayEvent.instance.onReset += OnReset;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onDeath -= OnDeath;
            GameplayEvent.instance.onReset -= OnReset;
        }
        private void Awake()
        {
            instance = this;
            wInput = new WInput();
        }
        public void DisableAll()
        {
            wInput.Disable();
        }
        private void OnReset()
        {
            wInput.Pause.Enable();
            wInput.Player.Enable();
            wInput.Gun.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnDeath()
        {
            print("DEATH");
            wInput.Pause.Disable();
            wInput.Player.Disable();
            wInput.Gun.Disable();
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
