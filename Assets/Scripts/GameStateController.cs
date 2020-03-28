using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public delegate void SceneAction();
    public static event SceneAction OnSceneChange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        OnSceneChange?.Invoke();
        SceneManager.LoadSceneAsync("Main_Menu");
    }
    
    public void LoadLobby()
    {
        OnSceneChange?.Invoke();
        SceneManager.LoadSceneAsync("Character_Select");
    }

    public AsyncOperation LoadStage()
    {
        OnSceneChange?.Invoke();
        return SceneManager.LoadSceneAsync("Main_Ship");
    }
}
