using System.Collections;
using System.Collections.Generic;
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
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlaySingle(titleJingle);
        soundManager.PlayMusic(menuMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
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
