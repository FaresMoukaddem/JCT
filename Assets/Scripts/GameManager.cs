using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SceneLoader sceneLoader;

    private int levelToLoad;

    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(this);
            instance = this;
            sceneLoader.OnGameSceneLoaded.AddListener(OnGameSceneLoaded);
            sceneLoader.OnMenuSceneLoaded.AddListener(OnMenuSceneLoaded);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        if (SoundHandler.instance)
        {
            SoundHandler.instance.PlaySong(1);
        }
    }

    public void LoadGameWithLevel(int levelIndex) 
    {
        levelToLoad = levelIndex;
        SoundHandler.instance.StopMusic();
        LoadGameScene();
    }

    public void OnGameSceneLoaded()
    {
        RoundManager.instance.StartLevel(levelToLoad);
        SoundHandler.instance.PlaySong(0);
    }

    public void OnMenuSceneLoaded()
    {
        SoundHandler.instance.PlaySong(1);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadGameWithLevel(1);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            LoadMenuScene();

        }
    }

    public void LoadMenuScene()
    {
        sceneLoader.LoadScene("Menu");
    }

    public void LoadGameScene()
    {
        sceneLoader.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
