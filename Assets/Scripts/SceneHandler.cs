using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    public void LoadEditor()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void LoadMain()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void LoadLevelManager()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
