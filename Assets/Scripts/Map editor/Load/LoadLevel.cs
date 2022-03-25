using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
using UnityEngine.UI;

namespace WEditor.Scenario
{
    public class LoadLevel : MonoBehaviour
    {
        [SerializeField] GameObject scrollViewContent;
        [SerializeField] GameObject levelPrefab;
        [SerializeField] WEditor.Scenario.Playable.ScenarioGenerator scenarioGenerator;
        private void OnEnable()
        {
            GameEvent.instance.onSrollViewEnable += LoadToScrollView;
            GameEvent.instance.onSrollViewDisable += UnloadScrollViewContent;
        }
        private void OnDisable()
        {
            GameEvent.instance.onSrollViewEnable -= LoadToScrollView;
            GameEvent.instance.onSrollViewDisable -= UnloadScrollViewContent;

        }
        public void LoadToScrollView()
        {
            List<GameData> levelData = ReadFromLocal();
            PutIntoContent(levelData);
        }
        private void PutIntoContent(List<GameData> levelData)
        {
            levelData.ForEach(level =>
            {
                GameObject newLevel = Instantiate(levelPrefab);

                Button buttonLevel = newLevel.GetComponent<Button>();
                TextMeshProUGUI contentText = newLevel.GetComponentInChildren<TextMeshProUGUI>();
                contentText.text = level.levelName;

                buttonLevel.onClick.AddListener(() =>
                {
                    scenarioGenerator.InitGeneration(new Vector3(level.levelSpawn.x, level.levelSpawn.z));
                });
                newLevel.transform.SetParent(scrollViewContent.transform);
                RectTransform rect =  newLevel.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
            });
        }

        public void UnloadScrollViewContent()
        {
            var childs = scrollViewContent.transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
            {
                Destroy(childs[i]);
            }
        }
        private List<GameData> ReadFromLocal()
        {
            string allPaths = PlayerPrefs.GetString("levelNames");
            string[] paths = allPaths.Split('_');
            List<GameData> gameData = new List<GameData>();
            for (int i = 0; i < paths.Length; i++)
            {
                gameData.Add(SaveData.LoadLocalLevel(paths[i]));
            }
            return gameData;
        }
    }
}