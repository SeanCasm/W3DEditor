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
        private void Start()
        {
            PauseInput.instance.EnableAndSetCallbacks(this);
        }
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started && PreviewHandler.onPreview)
            {
                switch (menuEnable)
                {
                    case ActualMenu.None:

                        pauseUI.SetActive(true);
                        PlayerControllerInput.instance.Disable();
                        GunInput.instance.Disable();
                        Cursor.lockState = CursorLockMode.None;

                        break;
                    case ActualMenu.Pause:

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
            PlayerControllerInput.instance.Enable();
            GunInput.instance.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
        public void Button_Resume()
        {
            Resume();
        }
        public void Button_Exit()
        {
            pauseUI.SetActive(false);
            GameEvent.instance.PreviewModeExit();
        }
    }
    public enum ActualMenu
    {
        Pause, Settings, None
    }
}
