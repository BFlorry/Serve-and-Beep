using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip menuMusic;

    [SerializeField]
    private AudioClip titleJingle;

    [SerializeField]
    private AudioClip navSfx;

    [SerializeField]
    private AudioClip clickSfx;

    [SerializeField]
    private GameObject optionsMenu;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Awake()
    {
        Start();
        Cursor.visible = true;
    }

    void Start()
    {
        StartCoroutine(WaitBeforePlayingBgm());
    }

    IEnumerator WaitBeforePlayingBgm()
    {
        // Check if timescale not 1 (if returning from stageover, time is 0)
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlaySingle(titleJingle);

        yield return new WaitForSeconds(2);

        soundManager.PlayMusic(menuMusic);

        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    public void NavSound()
    {
        if (navSfx != null)
        {
            soundManager.PlaySingle(navSfx);
        }
        else Debug.LogWarning("Couldn't play sound effect: " + navSfx + ". ", navSfx);
    }

    public void ClickSound()
    {
        if (clickSfx != null)
        {
            soundManager.PlaySingle(clickSfx);
        }
        else Debug.LogWarning("Couldn't play sound effect: " + clickSfx + ". ", clickSfx);
    }

    public void StartGame()
    {
        ClickSound();
        FindObjectOfType<GameStateController>().LoadLobby();
    }

    public void Options()
    {
        ClickSound();
        optionsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
