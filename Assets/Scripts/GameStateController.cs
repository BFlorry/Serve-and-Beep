using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public delegate void SceneAction();
    public static event SceneAction OnSceneChange;

    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out playerManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        OnSceneChange?.Invoke();
        SceneManager.LoadSceneAsync("Main_Menu");
        Time.timeScale = 1f;
    }
    
    public void LoadLobby()
    {
        OnSceneChange?.Invoke();
        SceneManager.LoadSceneAsync("Character_Select");
        Time.timeScale = 1f;
    }

    public void LoadStage(string scene)
    {
        OnSceneChange?.Invoke();
        Time.timeScale = 1f;
        playerManager.LoadScene(scene);
    }

    public void RestartScene()
    {
        Scene curScene = SceneManager.GetActiveScene();
        LoadStage(curScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
