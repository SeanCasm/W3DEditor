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
        [SerializeField] Loader loadType;
        private List<GameObject> levelsLoaded = new List<GameObject>();
        private void OnEnable()
        {
            GameEvent.instance.onEditorExit += ReloadScrollViewContent;
            PutIntoContent();
        }
        private void OnDisable()
        {
            levelsLoaded.Clear();
            GameEvent.instance.onEditorExit -= ReloadScrollViewContent;
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
                contentText.text = levelName;
                GameData newGamedata = gameDatas[i];
                buttonLevel.onClick.AddListener(() =>
                {
                    DataHandler.currentLevelName = levelName;
                    (int x, int z) levelSpawn = newGamedata.levelSpawn;
                    DataHandler.currentLevelPosition = new Vector3(levelSpawn.x, .5f, levelSpawn.z);
                    if (loadType == Loader.Play)
                    {
                        SceneHandler.instance.LoadPlayScene(newGamedata);
                    }
                    else
                    {
                        SceneHandler.instance.LoadEditorFromLoadOption(newGamedata);
                        levelsLoaded.Clear();
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
    public enum Loader
    {
        Play, Editor
    }
}