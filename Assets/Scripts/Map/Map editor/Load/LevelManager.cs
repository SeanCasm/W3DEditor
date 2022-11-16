using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using WEditor.UI;
using WEditor.Events;
namespace WEditor.Scenario
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] GameObject scrollViewContent;
        [SerializeField] GameObject levelPrefab, optionPanel;
        private Dictionary<string, LevelOption> levelsLoaded = new Dictionary<string, LevelOption>();
        private string currLevelName;
        private int delete = 0;
        private void OnEnable()
        {
            EditorEvent.instance.onEditorExit += ReloadScrollViewContent;
            PutIntoContent();
        }
        private void OnDisable()
        {
            levelsLoaded.Clear();
            EditorEvent.instance.onEditorExit -= ReloadScrollViewContent;
        }
        private void PutIntoContent()
        {
            GameData[] gameDatas = SaveData.LoadLocalLevels();
            foreach (var item in gameDatas)
            {
                string levelName = item.levelName;
                GameObject newLevel = Instantiate(levelPrefab);

                Button buttonLevel = newLevel.GetComponent<Button>();
                TextMeshProUGUI contentText = newLevel.GetComponentInChildren<TextMeshProUGUI>();
                contentText.text = levelName;
                LevelOption levelOption = buttonLevel.GetComponent<LevelOption>();
                levelOption.gameData = item;
                levelOption.currLevelSelected = levelName;
                buttonLevel.onClick.AddListener(() =>
                {
                    levelOption.currLevelSelected = item.levelName;
                    currLevelName = levelName;
                    optionPanel.SetActive(!optionPanel.activeSelf);
                });
                levelsLoaded.Add(levelName, levelOption);
                newLevel.transform.SetParent(scrollViewContent.transform, false);
                RectTransform rect = newLevel.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
            }
        }
        public void DeleteLevel()
        {
            delete++;
            if (delete == 2)
            {
                LevelOption level = levelsLoaded[currLevelName];
                level.DeleteLevel();
                optionPanel.SetActive(false);
                delete = 0;
            }

            MessageHandler.instance.SetMessage("level_del");
        }
        public void Load()
        {
            (int x, int z) levelSpawn;
            GameData gameData = levelsLoaded[currLevelName].gameData;
            levelSpawn.x = gameData.levelSpawnX;
            levelSpawn.z = gameData.levelSpawnZ;
            DataHandler.SetCurrentLevel(gameData);
            if (SceneHandler.instance.isPreEditorScene)
            {
                SceneHandler.instance.LoadEditorFromLoadOption(gameData);
                levelsLoaded.Clear();
            }
            else
            {
                SceneHandler.instance.LoadPlayScene(gameData);
            }
        }
        private void ReloadScrollViewContent()
        {
            levelsLoaded.Clear();
            PutIntoContent();
        }
    }
}