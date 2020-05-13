using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageOverManager : MonoBehaviour
{
    public delegate void StageOverAction();
    public static event StageOverAction OnStageOver;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private AudioClip[] starAudios;

    [SerializeField]
    private float[] fanfareStarTimes = new float[] { 0.0f, 1.0f, 2.0f, 3.5f };

    [SerializeField]
    private GameObject[] starImages;

    [SerializeField]
    private GameObject[] texts;

    private int stars = 0;

    private float dspFanfareTime = float.MaxValue;

    private SoundManager soundManager;

    [SerializeField]
    private AudioClip levelEndSfx;

    private bool enableButtonFunctionality = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        soundManager = FindObjectOfType<SoundManager>();
        int score = levelManager.LevelScore;
        scoreText.text = score.ToString();
        stars = levelManager.GetLevelStars();
        Cursor.visible = true;

        OnStageOver?.Invoke();
        StartCoroutine(WaitBeforeScorescreen());
    }

    private IEnumerator WaitBeforeScorescreen()
    {
        // TODO: This sound play should be done in LevelManager, but currently bugs when done so.
        soundManager.PlaySingle(levelEndSfx);

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < 1.5f)
        {
            yield return null;
        }
        BeginScoreScreen();
    }

    private void BeginScoreScreen()
    {
        dspFanfareTime = (float)AudioSettings.dspTime;
        soundManager.PlaySingleSecondary(starAudios[stars]);
    }

    void Update()
    {
        if (!soundManager.IsSFXPlaying())
        {
            switch (stars)
            {
                case 0:
                    if (fanfareStarTimes[1] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        texts[0].SetActive(true);
                        enableButtonFunctionality = true;
                        // Activate sad guy?
                    }
                    break;
                case 1:
                    if (fanfareStarTimes[1] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        starImages[1].SetActive(true);
                        texts[1].SetActive(true);
                        enableButtonFunctionality = true;
                    }
                    break;
                case 2:
                    if (fanfareStarTimes[1] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        starImages[1].SetActive(true);
                    }
                    if (fanfareStarTimes[2] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        starImages[2].SetActive(true);
                        texts[2].SetActive(true);
                        enableButtonFunctionality = true;
                    }
                    break;
                case 3:
                    if (fanfareStarTimes[1] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        starImages[1].SetActive(true);
                    }
                    if (fanfareStarTimes[2] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        starImages[2].SetActive(true);
                    }
                    if (fanfareStarTimes[3] <= (float)(AudioSettings.dspTime - dspFanfareTime))
                    {
                        starImages[3].SetActive(true);
                        texts[3].SetActive(true);
                        enableButtonFunctionality = true;
                    }
                    break;
            }
        }

        if (enableButtonFunctionality)
        {
            GameStateController gameStateController = FindObjectOfType<GameStateController>();
            restartButton.onClick.AddListener(() => gameStateController.RestartScene());
            menuButton.onClick.AddListener(() => gameStateController.LoadMenu());
            nextButton.onClick.AddListener(() => gameStateController.NextStage());

            if (Input.GetKeyDown(KeyCode.N))
                gameStateController.NextStage();
            if (Input.GetKeyDown(KeyCode.L))
                gameStateController.LoadMenu();
            if (Input.GetKeyDown(KeyCode.R))
                gameStateController.RestartScene();
        }
    }
}
