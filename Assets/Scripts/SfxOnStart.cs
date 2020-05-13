using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxOnStart : MonoBehaviour
{

    [SerializeField]
    private AudioClip startSfx;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlaySingle(startSfx);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
