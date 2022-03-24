using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace WEditor.Scenario
{
    public class LoadLevel : MonoBehaviour
    {
        [SerializeField] GameObject scrollViewContent;
        [SerializeField] AssetReference levelButton;
        [SerializeField] WEditor.Scenario.Playable.ScenarioGenerator scenarioGenerator;
        GameObject content;
        private void Start()
        {
            levelButton.LoadAssetAsync<GameObject>().Completed += OnLoadComplete;
        }
        private void OnEnable()
        {
            GameEvent.instance.onSrollViewEnable += LoadToScrollView;
            GameEvent.instance.onSrollViewDisable += UnloadScrollViewContent;
        }
        private void OnDisable()
        {
            GameEvent.instance.onSrollViewEnable -= LoadToScrollView;
            GameEvent.instance.onSrollViewDisable -= UnloadScrollViewContent;
            levelButton.LoadAssetAsync<GameObject>().Completed -= OnLoadComplete;

        }
        private void OnLoadComplete(AsyncOperationHandle<GameObject> resultOperation)
        {
            content = resultOperation.Result;
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
                TextMeshProUGUI contentText = content.GetComponentInChildren<TextMeshProUGUI>();
                contentText.text = level.levelName;

                Button button = content.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    scenarioGenerator.InitGeneration(new Vector3(level.levelSpawn.x, level.levelSpawn.z));
                });
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