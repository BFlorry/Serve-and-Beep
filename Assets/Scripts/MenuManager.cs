using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip menuMusic;

    [SerializeField]
    private AudioClip navSfx;

    [SerializeField]
    private AudioClip clickSfx;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlayMusic(menuMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavSound()
    {
        soundManager.PlaySingle(navSfx);
    }

    public void ClickSound()
    {
        soundManager.PlaySingle(clickSfx);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenCredits()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
