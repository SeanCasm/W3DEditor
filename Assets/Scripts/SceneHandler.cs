using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WEditor;
using WEditor.Game;
using WEditor.Events;
using WEditor.Scenario.Editor;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    private GameData gameData;
    int width, height;
    public bool isEditorScene => SceneManager.GetActiveScene().buildIndex == 3 ? true : false;
    public bool isPreEditorScene => SceneManager.GetActiveScene().buildIndex == 1 ? true : false;
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
        Music.current.SetTitleTheme();
    }
    public void LoadEndGameScene()
    {
        SceneManager.LoadSceneAsync(4);
        Music.current.SetLevelEndTheme();
    }
    public void LoadMain()
    {
        SceneManager.LoadSceneAsync(0);
        Music.current.SetTitleTheme();
    }
    public void LoadPlayScene(GameData gameData)
    {
        this.gameData = gameData;
        Music.current.SetLevelTheme();
        SceneManager.LoadSceneAsync(2).completed += PlaySceneLoaded;
    }
    private void PlaySceneLoaded(AsyncOperation operation)
    {
        Music.current.SetAnotherTheme(this.gameData.levelMusicTheme);
        WEditor.Scenario.Playable.ScenarioGenerator scenarioGenerator = GameObject.FindObjectOfType<WEditor.Scenario.Playable.ScenarioGenerator>();
        scenarioGenerator.InitGeneration(gameData);
        EditorEvent.instance.PlayModeEnter();
    }
    private void EditorLoadCompleteCreate(AsyncOperation op)
    {
        StartCoroutine(WaitForEditorLoads(() => { EditorGrid.instance.Create(width, height); }));
    }
    private void EditorLoadCompleteLoad(AsyncOperation op)
    {
        Music.current.SetEditorTheme();
        StartCoroutine(WaitForEditorLoads(() => { EditorGrid.instance.Load(gameData); }));
    }
    IEnumerator WaitForEditorLoads(System.Action callback)
    {
        while (EditorGrid.instance == null)
        {
            yield return null;
        }
        Music.current.SetEditorTheme();
        callback();
    }
}
