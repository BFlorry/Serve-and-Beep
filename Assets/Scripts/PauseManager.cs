using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public static bool GameIsPaused { get; private set; } = false;


    public void TogglePause()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main_Menu"))
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
