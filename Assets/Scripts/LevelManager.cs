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
    private int[] requiredScore = new int[]{750, 1500, 2800};

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private AudioClip bgmMusic;

    [SerializeField]
    private AudioClip timeRunningOutSfx;

    [SerializeField]
    private AudioClip timeOverSfx;

    [SerializeField]
    private AudioClip posReviewSfx;

    [SerializeField]
    private AudioClip negReviewSfx;

    [SerializeField]
    private AudioClip stageOverMusic;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject[] playerSpawnpoints;

    private SoundManager soundManager;

    public GameObject[] PlayerSpawnpoints { get => playerSpawnpoints; }
    public int LevelScore { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        //soundManager = FindObjectOfType<SoundManager>();
        levelState = LevelPhase.Start;
        timeLeft = levelTimeLimit;
        LevelScore = 0;
        scoreText.text = LevelScore.ToString();
        Time.timeScale = 1f;
    }

    public int GetLevelStars()
    {
        int starCount = 0;
        foreach(int starLimit in requiredScore)
        {
            if (LevelScore >= starLimit) starCount++;
        }
        return starCount;
    }

    public void ChangeScore(int amount)
    {
        if (amount > 0) soundManager.PlaySingle(posReviewSfx);
        else soundManager.PlaySingle(negReviewSfx);
        if((LevelScore + amount) >= 0)
        {
            LevelScore += amount;
        }
        else if((LevelScore + amount) < 0)
        {
            LevelScore = 0;
        }
        scoreText.text = LevelScore.ToString();
    }

    private void Update()
    {
        #if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.PageUp)) ChangeScore(100);
        if (Input.GetKeyDown(KeyCode.PageDown)) ChangeScore(-100);
        #endif

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
                    soundManager.StopMusic();
                    Time.timeScale = 0f;
                    // TODO: Figure out why PlaySingle bugs out here! An unity bug?
                    //soundManager.PlaySingle(timeOverSfx);
                    //soundManager.PlayMusic(stageOverMusic);
                    gameOverMenu.SetActive(true);
                    break;
                }
        }
    }
}
