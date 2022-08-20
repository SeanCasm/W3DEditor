using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using WEditor.Scenario.Editor;
using System;
using WEditor.Events;

namespace WEditor.Scenario
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] GameObject scrollViewContent;
        [SerializeField] GameObject levelPrefab;
        private List<GameObject> levelsLoaded = new List<GameObject>();
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

            for (int i = 0; i < gameDatas.Length; i++)
            {
                string levelName = gameDatas[i].levelName;
                GameObject newLevel = Instantiate(levelPrefab);

                Button buttonLevel = newLevel.GetComponent<Button>();
                TextMeshProUGUI contentText = newLevel.GetComponentInChildren<TextMeshProUGUI>();
                contentText.fontSize = levelName.Length >= 7 ? 14 : 18;
                contentText.text = levelName;
                GameData newGamedata = gameDatas[i];
                buttonLevel.onClick.AddListener(() =>
                {
                    (int x, int z) levelSpawn;
                    levelSpawn.x = newGamedata.levelSpawnX;
                    levelSpawn.z = newGamedata.levelSpawnZ;
                    DataHandler.SetCurrentLevel(newGamedata);
                    if (SceneHandler.instance.isPreEditorScene)
                    {
                        SceneHandler.instance.LoadEditorFromLoadOption(newGamedata);
                        levelsLoaded.Clear();
                    }
                    else
                    {
                        SceneHandler.instance.LoadPlayScene(newGamedata);
                    }
                });
                levelsLoaded.Add(newLevel);
                newLevel.transform.SetParent(scrollViewContent.transform, false);
                RectTransform rect = newLevel.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
            }
        }
        private void ReloadScrollViewContent()
        {
            levelsLoaded.Clear();
            PutIntoContent();
        }
    }
}