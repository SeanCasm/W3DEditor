using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WEditor.Events;
using static WInput;

namespace WEditor.UI
{
    public class Pause : MonoBehaviour, IPauseActions
    {
        [SerializeField] GameObject pauseUI;
        [SerializeField] GameObject settingsMenu;
        private ActualMenu menuEnable = ActualMenu.None;
        private void OnEnable()
        {
            EditorEvent.instance.onPreviewModeEnter += EnableInput;
            EditorEvent.instance.onPreviewModeExit += DisableInput;
            EditorEvent.instance.onPlayModeEnter += EnableInput;
            EditorEvent.instance.onPlayModeExit += DisableInput;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onPreviewModeEnter -= EnableInput;
            EditorEvent.instance.onPreviewModeExit -= DisableInput;
            EditorEvent.instance.onPlayModeEnter -= EnableInput;
            EditorEvent.instance.onPlayModeExit -= DisableInput;
        }
        private void EnableInput()
        {
            PauseInput.instance.EnableAndSetCallbacks(this);
        }
        private void DisableInput()
        {
            PauseInput.instance.Disable();
        }
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                switch (menuEnable)
                {
                    case ActualMenu.None:

                        pauseUI.SetActive(true);
                        PlayerControllerInput.instance.Disable();
                        GunInput.instance.Disable();
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                        menuEnable = ActualMenu.Pause;
                        break;
                    case ActualMenu.Pause:

                        menuEnable = ActualMenu.None;
                        Resume();

                        break;
                    case ActualMenu.Settings:

                        pauseUI.SetActive(true);

                        break;
                }
            }
        }
        private void Resume()
        {
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            PlayerControllerInput.instance.Enable();
            GunInput.instance.Enable();
        }
        public void Button_Resume()
        {
            Resume();
        }
        public void Button_Exit()
        {
            pauseUI.SetActive(false);
            EditorEvent.instance.PreviewModeExit();
            Time.timeScale = 1;
        }
        public void Button_ExitPlayMode()
        {
            EditorEvent.instance.PlayModeExit();
            SceneHandler.instance.LoadMain();
            Time.timeScale = 1;
        }
    }
    public enum ActualMenu
    {
        Pause, Settings, None
    }
}
