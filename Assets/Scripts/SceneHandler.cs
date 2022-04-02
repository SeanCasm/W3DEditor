using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WEditor.Scenario;
using WEditor;
using WEditor.Scenario.Playable;
using WEditor.Events;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    private GameData gameData;
    private void Start()
    {
        instance = this;
    }
    public void LoadEditor()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void LoadMain()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void LoadPlayScene(GameData gameData)
    {
        this.gameData = gameData;
        SceneManager.LoadSceneAsync(2).completed += PlaySceneLoaded;
    }
    private void PlaySceneLoaded(AsyncOperation operation)
    {
        ScenarioGenerator scenarioGenerator = GameObject.FindObjectOfType<ScenarioGenerator>();
        scenarioGenerator.InitGeneration(gameData);
        GameEvent.instance.PlayModeEnter();
    }
}
