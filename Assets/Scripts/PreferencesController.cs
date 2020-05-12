using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PreferencesController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    Resolution[] resolutions;
    private int activeResolutionIndex = -1;

    private void OnEnable()
    {
        LoadOptions();
    }

    private void LoadOptions()
    {
        float masterVolume;
        float bgmVolume;
        float sfxVolume;
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetVolume(masterVolume);

        }
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
            SetBGMVolume(bgmVolume);

        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume(sfxVolume);

        }

        if (PlayerPrefs.HasKey("Quality"))
        {
            int quality = PlayerPrefs.GetInt("Quality");
            SetQuality(quality);
        }
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
            SetFullscreen(isFullscreen);
        }

        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();
        if (PlayerPrefs.HasKey("Resolution"))
        {
            activeResolutionIndex = PlayerPrefs.GetInt("Resolution");
            SetResolution(activeResolutionIndex);
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
