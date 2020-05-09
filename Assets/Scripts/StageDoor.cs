using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDoor : MonoBehaviour
{

    [SerializeField]
    private AudioClip levelEnterSfx;

    private SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if(soundManager == null) soundManager = FindObjectOfType<SoundManager>();
            soundManager.PlaySingle(levelEnterSfx);
        }
    }
}
