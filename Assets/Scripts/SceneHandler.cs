using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WEditor;
using WEditor.Events;
using WEditor.Scenario.Editor;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    private GameData gameData;
    int width, height;
    private void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void Button_LoadEditorFromCreateOption(int width, int height)
    {
        this.width = width;
        this.height = height;
        SceneManager.LoadSceneAsync(3).completed += EditorLoadCompleteCreate;
    }
    public void LoadEditorFromLoadOption(GameData gameData)
    {
        this.gameData = gameData;
        SceneManager.LoadSceneAsync(3).completed += EditorLoadCompleteLoad;
    }
    public void LoadPreMapEditor()
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
        WEditor.Scenario.Playable.ScenarioGenerator scenarioGenerator = GameObject.FindObjectOfType<WEditor.Scenario.Playable.ScenarioGenerator>();
        scenarioGenerator.InitGeneration(gameData);
        GameEvent.instance.PlayModeEnter();
    }
    private void EditorLoadCompleteCreate(AsyncOperation op)
    {
        StartCoroutine(WaitForEditorLoads(() => { EditorGrid.instance.Create(width, height); }));
    }
    private void EditorLoadCompleteLoad(AsyncOperation op)
    {
        StartCoroutine(WaitForEditorLoads(() => { EditorGrid.instance.Load(gameData); }));
    }
    IEnumerator WaitForEditorLoads(System.Action callback)
    {
        while (EditorGrid.instance == null)
        {
            yield return null;
        }
        callback();
    }
}
