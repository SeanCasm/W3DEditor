using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
using UnityEngine.UI;
using WEditor.Scenario.Editor;
using System;
namespace WEditor.Scenario
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] GameObject scrollViewContent;
        [SerializeField] GameObject levelPrefab;
        [SerializeField] Loader loadType;
        private Action load;
        private void OnEnable()
        {
            GameEvent.instance.onSrollViewEnable += PutIntoContent;
            GameEvent.instance.onSrollViewDisable += UnloadScrollViewContent;
        }
        private void OnDisable()
        {
            GameEvent.instance.onSrollViewEnable -= PutIntoContent;
            GameEvent.instance.onSrollViewDisable -= UnloadScrollViewContent;
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
                    DataHandler.currentLevelPosition = new Vector3Int(levelSpawn.x, levelSpawn.z, 0);
                    if (loadType == Loader.LoadPlay)
                    {
                        SceneHandler.instance.LoadPlayScene(newGamedata);
                    }
                    else
                    {
                        GameEvent.instance.EditorEnter();
                        EditorGrid.instance.Load(newGamedata);
                    }
                });
                newLevel.transform.SetParent(scrollViewContent.transform);
                RectTransform rect = newLevel.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
            }
        }
        public void UnloadScrollViewContent()
        {
            var childs = scrollViewContent.transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
            {
                Destroy(childs[i]);
            }
        }
    }
    public enum Loader
    {
        LoadPlay, LoadEditor
    }
}