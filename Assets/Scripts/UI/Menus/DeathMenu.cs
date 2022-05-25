using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;

namespace WEditor.UI
{
    public class DeathMenu : MonoBehaviour
    {
        public static DeathMenu instance;
        [SerializeField] GameObject deathMenu;
        [SerializeField] bool isFromEditor;
        private void Start()
        {
            instance = this;
        }
        public void Enable()
        {
            deathMenu.SetActive(true);
        }
        public void Exit()
        {
            if (isFromEditor)
            {
                SceneHandler.instance.LoadPreMapEditor();
            }
            else
            {
                SceneHandler.instance.LoadMain();
            }
        }
        public void Reset()
        {
            Time.timeScale = 1;
            StatusBehaviour.instance.Respawn();
        }
        private void OnDisable()
        {
            deathMenu.SetActive(false);
        }
    }
}
