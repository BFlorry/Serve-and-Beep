using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    //Drag a reference to the audio source which will play the sound effects.
    public AudioSource sfxSource;
    public AudioSource secondarySfxSource;
    //Drag a reference to the audio source which will play the music.
    public AudioSource musicSource;
    public AudioSource secondaryMusicSource;
    //Allows other scripts to call functions from SoundManager.
    public static SoundManager instance = null;
    //The lowest a sound effect will be randomly pitched.
    public float lowPitchRange = .95f;
    //The highest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;

    private void OnEnable()
    {
        GameStateController.OnSceneChange += UnregisterSounds;
    }
    private void OnDisable()
    {
        GameStateController.OnSceneChange -= UnregisterSounds;
    }
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Play the clip.
        if(sfxSource != null) sfxSource.PlayOneShot(clip);
        else Debug.LogWarning("SFX source was null: ", sfxSource);
    }

    //Used to play single sound clips.
    public void PlaySingleSecondary(AudioClip clip)
    {
        //Play the clip.
        if (secondarySfxSource != null) secondarySfxSource.PlayOneShot(clip);
        else Debug.LogWarning("Secondary SFX source was null: ", secondarySfxSource);
    }

    public void PlaySingleStoppable(AudioClip clip)
    {
        //Play the clip.
        if (sfxSource != null)
        {
            sfxSource.clip = clip;
            sfxSource.Play();
        }
        else Debug.LogWarning("SFX source was null: ", sfxSource);
    }

    public bool IsSFXPlaying()
    {
        //Play the clip.
        if (sfxSource != null)
        {
            return sfxSource.isPlaying;
        }
        else Debug.LogWarning("SFX source was null: ", sfxSource);
        return false;
    }
    public void StopSingleStoppable()
    {
        //Stop the clip.
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
        else Debug.LogWarning("SFX source was null: ", sfxSource);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null)
        {
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            musicSource.clip = clip;

            //Play the clip.
            musicSource.Play();
        }
        else Debug.LogWarning("Music source was null: ", musicSource);
    }

    public void PlaySecondaryMusic(AudioClip clip)
    {
        if(secondaryMusicSource != null)
        {
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            secondaryMusicSource.clip = clip;

            //Play the clip.
            secondaryMusicSource.Play();
        }
        else Debug.LogWarning("Music source was null: ", secondaryMusicSource);
    }

    public void StopMusic()
    {
        //Stop the clip.
        if (musicSource != null) musicSource.Stop();
        else Debug.LogWarning("Music source was null: ", musicSource);
    }

    public void StopSecondaryMusic()
    {
        //Stop the clip.
        if (secondaryMusicSource != null) secondaryMusicSource.Stop();
        else Debug.LogWarning("Music source was null: ", secondaryMusicSource);
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
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
        sfxSource.Play();
    }

    void UnregisterSounds()
    {
        //if(sfxSource.isPlaying) sfxSource.Stop();
        //if(musicSource.isPlaying) musicSource.Stop();
        //if(secondaryMusicSource.isPlaying) secondaryMusicSource.Stop();
    }
}