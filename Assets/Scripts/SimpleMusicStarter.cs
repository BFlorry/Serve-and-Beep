using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMusicStarter : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlayMusic(music);
    }

}
