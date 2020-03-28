﻿using System.Collections;
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
    void Awake()
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
        if(navSfx != null) soundManager.PlaySingle(navSfx);
    }

    public void ClickSound()
    {
        soundManager.PlaySingle(clickSfx);
    }

    public void StartGame()
    {
        FindObjectOfType<GameStateController>().LoadLobby();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
