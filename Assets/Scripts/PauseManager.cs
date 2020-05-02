using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public delegate void PauseAction();
    public static event PauseAction OnPause;
    public static event PauseAction OnResume;

    public GameObject pauseMenuUI;

    public static bool GameIsPaused { get; private set; } = false;

    public AudioMixerSnapshot levelSnapshot;

    public AudioMixerSnapshot pauseSnapshot;

    public AudioClip pauseSfx;

    public AudioClip resumeSfx;

    public AudioSource Audio;

    [SerializeField]
    private GameObject options;

    private GameStateController gameStateController;

    private void Awake()
    {
        gameStateController = GetComponent<GameStateController>();
    }

    public void TogglePause()
    {
        // Don't toggle pause if current scene is main menu and that StageOverManager is not enabled
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main_Menu")
            && !FindObjectOfType<StageOverManager>() && !options.activeSelf)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        gameStateController.DisableEventSystem();
        Audio.PlayOneShot(resumeSfx);
        levelSnapshot.TransitionTo(0.5f);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        OnResume?.Invoke();
    }

    void Pause()
    {
        gameStateController.EnableEventSystem();
        Audio.PlayOneShot(pauseSfx);
        pauseSnapshot.TransitionTo(0.5f);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        OnPause?.Invoke();
    }

    public void LoadMenu()
    {
        Resume();
        //SceneManager.LoadScene("Main_Menu");
        FindObjectOfType<GameStateController>().LoadMenu();
    }

    public void Options()
    {
        options.GetComponent<OptionsMenu>().PauseMenu = pauseMenuUI;
        options.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        FindObjectOfType<GameStateController>().QuitGame();
    }
}
