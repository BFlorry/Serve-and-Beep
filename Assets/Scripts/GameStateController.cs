using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public delegate void SceneAction();
    public static event SceneAction OnSceneChange;
    public static event SceneAction OnSceneRestart;

    private PlayerManager playerManager;

    private GameObject eventSystem;

    private PlayerInput mainMenuInput;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current.gameObject;
        TryGetComponent(out playerManager);
        StageOverManager.OnStageOver += EnableEventSystem;
        mainMenuInput = eventSystem.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableEventSystem()
    {
        eventSystem.SetActive(true);
    }

    private void EnablePlayerInput()
    {
        mainMenuInput.enabled = true;
    }

    public void DisableEventSystem()
    {
        mainMenuInput.enabled = false;
        eventSystem.SetActive(false);
    }

    public void LoadMenu()
    {
        OnSceneChange?.Invoke();
        playerManager.DeserializePlayers();
        SceneManager.LoadSceneAsync("Main_Menu");
        Time.timeScale = 1f;
        EnableEventSystem();
        //EnablePlayerInput();
    }
    
    public void LoadLobby()
    {
        OnSceneChange?.Invoke();
        SceneManager.LoadSceneAsync("Character_Select");
        Time.timeScale = 1f;
        DisableEventSystem();
    }

    public void LoadStage(string scene)
    {
        OnSceneChange?.Invoke();
        Time.timeScale = 1f;
        playerManager.LoadScene(scene);
        DisableEventSystem();
    }

    public void RestartScene()
    {
        OnSceneRestart?.Invoke();
        Scene curScene = SceneManager.GetActiveScene();
        LoadStage(curScene.name);
        DisableEventSystem();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
