using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.UI
{
    public class DeathMenu : MonoBehaviour
    {
        public static DeathMenu instance;
        [SerializeField] GameObject deathMenu;
        [SerializeField] bool isFromEditor;
        private void OnEnable()
        {
            GameplayEvent.instance.onDeath += OnDeath;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onReset -= OnDeath;
        }
        private void Start()
        {
            instance = this;
        }
        public void Exit()
        {
            if (isFromEditor)
            {
                PreviewHandler.instance.OnEdit();
            }
            else
            {
                SceneHandler.instance.LoadMain();
            }
        }
        private void OnDeath()
        {
            deathMenu.SetActive(true);
        }
        public void Reset()
        {
            Time.timeScale = 1;
            SceneHandler.instance.LoadPlayScene(DataHandler.currentLevel);
            GameplayEvent.instance.OnReset();
            deathMenu.SetActive(false);
        }
    }
}
