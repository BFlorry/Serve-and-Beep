using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider masterVolumeSlider;
    [SerializeField]
    private Slider bgmVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private TMP_Dropdown qualityDropdown;
    [SerializeField]
    private Toggle fullscreenToggle;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private AudioClip sliderSfx;
    [SerializeField]
    private AudioClip checkBoxSfx;
    [SerializeField]
    private AudioClip exitSfx;
    

    private SoundManager soundManager;
    Resolution[] resolutions;

    private int activeResolutionIndex = -1;

    public GameObject PauseMenu { get; set; }

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
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        else
            audioMixer.GetFloat("MasterVolume", out masterVolume);

        if (PlayerPrefs.HasKey("BGMVolume"))
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        else
            audioMixer.GetFloat("BGMVolume", out bgmVolume);

        if (PlayerPrefs.HasKey("SFXVolume"))
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        else
            audioMixer.GetFloat("SFXVolume", out sfxVolume);

        SetVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
        masterVolumeSlider.value = masterVolume;
        bgmVolumeSlider.value = bgmVolume;
        sfxVolumeSlider.value = sfxVolume;

        if (PlayerPrefs.HasKey("Quality"))
        {
            int quality = PlayerPrefs.GetInt("Quality");
            SetQuality(quality);
            qualityDropdown.value = quality;
        }
        else
        {
            qualityDropdown.value = QualitySettings.GetQualityLevel();
        }
        qualityDropdown.RefreshShownValue();

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
            SetFullscreen(isFullscreen);
            fullscreenToggle.isOn = isFullscreen;
        }
        else
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }

        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();
        if (PlayerPrefs.HasKey("Resolution"))
        {
            activeResolutionIndex = PlayerPrefs.GetInt("Resolution");
            SetResolution(activeResolutionIndex);
        }
        foreach (Resolution reso in resolutions)
        {
            string option = reso.width + "x" + reso.height;
            resolutionOptions.Add(option);

            // Check if resolution has not been loaded and that current resolution is current resolution
            if (activeResolutionIndex == -1 && (reso.width == Screen.currentResolution.width && reso.height == Screen.currentResolution.height))
            {
                activeResolutionIndex = resolutionOptions.FindIndex(x => x.Equals(option));
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = activeResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlaySingle(sliderSfx);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        soundManager.PlaySingle(checkBoxSfx);
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", System.Convert.ToInt32(isFullscreen));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void BackButton()
    {
        PauseMenu.SetActive(true);
        soundManager.PlaySingle(exitSfx);
        this.gameObject.SetActive(false);
    }
}
