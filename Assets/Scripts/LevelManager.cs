using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    enum LevelPhase
    {
        Start,
        Normal,
        HurryupBegin,
        Hurryup,
        Finish
    }

    private LevelPhase levelState;

    [SerializeField]
    private float levelTimeLimit = 180f;

    private float timeLeft = -1f;

    [SerializeField]
    private float hurryUpTime = 170f;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private AudioClip bgmMusic;

    [SerializeField]
    private AudioClip timeRunningOutSfx;

    [SerializeField]
    private AudioClip timeOverSfx;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //soundManager = FindObjectOfType<SoundManager>();
        levelState = LevelPhase.Start;
        timeLeft = levelTimeLimit;
    }

    private void Update()
    {
        if (levelState != LevelPhase.Finish)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = timeLeft.ToString("F0");
        }

        switch (levelState)
        {
            case LevelPhase.Start:
                {
                    soundManager = FindObjectOfType<SoundManager>();
                    soundManager.PlayMusic(bgmMusic);
                    levelState = LevelPhase.Normal;
                    break;
                }
            case LevelPhase.Normal:
                {
                    if (timeLeft <= hurryUpTime)
                    {
                        levelState = LevelPhase.HurryupBegin;
                    }
                    break;
                }
            case LevelPhase.HurryupBegin:
                {
                    soundManager.PlaySecondaryMusic(timeRunningOutSfx);
                    levelState = LevelPhase.Hurryup;
                    break;
                }
            case LevelPhase.Hurryup:
                {
                    if (timeLeft <= 0f) levelState = LevelPhase.Finish;
                    break;
                }
            case LevelPhase.Finish:
                {
                    soundManager.StopSecondaryMusic();
                    soundManager.PlaySingle(timeOverSfx);
                    break;
                }
        }
    }
}
