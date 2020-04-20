using UnityEngine;
using System.Collections;

public class PlayerSfxManager : MonoBehaviour
{
    public AudioSource sfxSource;

    //The lowest a sound effect will be randomly pitched.
    public float lowPitchRange = .95f;
    //The highest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;


    void Awake()
    {
        //TODO: Decide whether to remove this commented section or not
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        //DontDestroyOnLoad(gameObject);
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Play the clip.
        sfxSource.pitch = 1.00f;
        sfxSource.PlayOneShot(clip);
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void PlayRandomized(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        sfxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        sfxSource.clip = clips[randomIndex];

        //Play the clip.
        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}