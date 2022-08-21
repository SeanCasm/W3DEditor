using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Scenario.Editor;
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
                deathMenu.SetActive(false);
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
            if (isFromEditor)
                EditorGrid.instance.InitGeneration();
            else
                SceneHandler.instance.LoadPlayScene(DataHandler.currentLevel);

            GameplayEvent.instance.OnReset();
            deathMenu.SetActive(false);
        }
    }
}
