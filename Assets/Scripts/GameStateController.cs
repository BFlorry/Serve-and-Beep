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
    public static GameStateController Instance { get; private set; }
    
    // Scene that lobby loads
    public string SceneToLoad { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current.gameObject;
        TryGetComponent(out playerManager);
        StageOverManager.OnStageOver += EnableEventSystem;
        //mainMenuInput = eventSystem.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableEventSystem()
    {
       // eventSystem.SetActive(true);
    }

    private void EnablePlayerInput()
    {
        //mainMenuInput.enabled = true;
        Cursor.visible = true;
    }

    public void DisableEventSystem()
    {
        //mainMenuInput.enabled = false;
        //eventSystem.SetActive(false);
        Cursor.visible = false;
    }

    public void LoadMenu()
    {
        OnSceneChange?.Invoke();
        TryGetComponent(out playerManager);
        playerManager.DeserializePlayers();
        SceneManager.LoadSceneAsync("Main_Menu");
        Time.timeScale = 1f;
        EnableEventSystem();
        EnablePlayerInput();
    }

    public void LoadLevelSelect()
    {
        OnSceneChange?.Invoke();
        TryGetComponent(out playerManager);
        playerManager.DeserializePlayers();
        SceneManager.LoadSceneAsync("Level_Select");
        Time.timeScale = 1f;
        EnableEventSystem();
        EnablePlayerInput();
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
        TryGetComponent(out playerManager);
        playerManager.LoadScene(scene);
        DisableEventSystem();
    }

    public void LoadStage(int scene)
    {
        OnSceneChange?.Invoke();
        Time.timeScale = 1f;
        TryGetComponent(out playerManager);
        playerManager.LoadScene(scene);
        DisableEventSystem();
    }

    public void NextStage()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //int nextSceneIndex = SceneManager.GetSceneByBuildIndex(curSceneIndex + 1);
        int nextSceneIndex = curSceneIndex + 1;

        LoadStage(nextSceneIndex);
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
