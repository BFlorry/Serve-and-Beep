using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public static bool GameIsPaused { get; private set; } = false;

    public AudioMixerSnapshot levelSnapshot;

    public AudioMixerSnapshot pauseSnapshot;

    public AudioClip pauseSfx;

    public AudioClip resumeSfx;

    public AudioSource Audio;

    [SerializeField]
    private GameObject options;



    public void TogglePause()
    {
        // Don't toggle pause if current scene is main menu and that StageOverManager is not enabled
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main_Menu")
            && !FindObjectOfType<StageOverManager>())
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
        Audio.PlayOneShot(resumeSfx);
        levelSnapshot.TransitionTo(0.5f);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Audio.PlayOneShot(pauseSfx);
        pauseSnapshot.TransitionTo(0.5f);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void LoadMenu()
    {
        Resume();
        //SceneManager.LoadScene("Main_Menu");
        FindObjectOfType<GameStateController>().LoadMenu();
    }

    public void Options()
    {
        options.SetActive(true);
    }

    public void QuitGame()
    {
        FindObjectOfType<GameStateController>().QuitGame();
    }
}
