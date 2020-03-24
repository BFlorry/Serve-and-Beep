using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private string _fadeScene;
    private float _alpha;

    private CanvasGroup _myCanvas;
    private Image _bg;
    private float _lastTime;
    private bool _startedLoading;

    private float _fadeInDuration;

    //Set callback
    private void OnEnable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    //Remove callback
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void InitiateFader(CanvasGroup canvasGroup, Image image, string scene, Color fadeColor, float fadeInDuration, float fadeOutDuration)
    {
        DontDestroyOnLoad(gameObject);

        _fadeInDuration = fadeInDuration;
        _fadeScene = scene;

        //Getting the visual elements
        _myCanvas = canvasGroup;
        _bg = image;
        _bg.color = fadeColor;

        //Checking and starting the coroutine
        _myCanvas.alpha = 0.0f;
        StartCoroutine(FadeIt(FadeDirection.Out, fadeOutDuration));
    }

    public void InitiateFadeIn(CanvasGroup canvasGroup, Image image, Color fadeColor, float fadeInDuration)
    {
        DontDestroyOnLoad(gameObject);

        _fadeInDuration = fadeInDuration;

        //Getting the visual elements
        _myCanvas = canvasGroup;
        _bg = image;
        _bg.color = fadeColor;

        //Checking and starting the coroutine
        _myCanvas.alpha = 0.0f;
        StartCoroutine(FadeIt(FadeDirection.In, fadeInDuration));
    }

    public void InitiateFadeOut(CanvasGroup canvasGroup, Image image, string scene, Color fadeColor, float fadeOutDuration)
    {
        DontDestroyOnLoad(gameObject);

        _fadeInDuration = fadeOutDuration;
        _fadeScene = scene;

        //Getting the visual elements
        _myCanvas = canvasGroup;
        _bg = image;
        _bg.color = fadeColor;

        //Checking and starting the coroutine
        _myCanvas.alpha = 0.0f;
        StartCoroutine(FadeIt(FadeDirection.Out, fadeOutDuration));
    }

    private enum FadeDirection
    {
        In,
        Out
    }

    private IEnumerator FadeIt(FadeDirection fadeDirection, float fadeDuration)
    {
        var timePassed = 0.0f;

        switch (fadeDirection)
        {
            case FadeDirection.Out:
                do
                {
                    _alpha = Mathf.Lerp(0, 1, timePassed / fadeDuration);
                    _myCanvas.alpha = _alpha;

                    timePassed += Time.deltaTime;
                    yield return null;
                } while (timePassed < fadeDuration);

                _alpha = 1;

                SceneManager.LoadSceneAsync(_fadeScene);
                break;

            case FadeDirection.In:
                do
                {
                    _alpha = Mathf.Lerp(1, 0, timePassed / fadeDuration);
                    _myCanvas.alpha = _alpha;

                    timePassed += Time.deltaTime;
                    yield return null;
                } while (timePassed < fadeDuration);

                _alpha = 0;

                Initiate.DoneFading();

                Debug.Log("Your scene has been loaded , and fading in has just ended");

                Destroy(gameObject);
                break;
        }
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //We can now fade in
        StartCoroutine(FadeIt(FadeDirection.In, _fadeInDuration));
    }
}