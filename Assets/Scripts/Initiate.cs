using UnityEngine;
using UnityEngine.UI;

public static class Initiate
{
    private static bool areWeFading;

    //Create Fader object and assing the fade scripts and assign all the variables
    public static void Fade(string scene, Color col, float fadeOutDuration, float fadeInDuration)
    {
        if (areWeFading)
        {
            Debug.Log("Already Fading");
            return;
        }

        var init = new GameObject("Fader", typeof(Canvas), typeof(CanvasGroup), typeof(Image), typeof(Fader));
        init.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        var fader = init.GetComponent<Fader>();
        areWeFading = true;
        fader.InitiateFader(init.GetComponent<CanvasGroup>(), init.GetComponent<Image>(), scene, col, fadeOutDuration, fadeInDuration);
    }

    public static void FadeIn(Color col, float fadeInDuration)
    {
        if (areWeFading)
        {
            Debug.Log("Already Fading");
            return;
        }

        var init = new GameObject("Fader", typeof(Canvas), typeof(CanvasGroup), typeof(Image), typeof(Fader));
        init.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        var fader = init.GetComponent<Fader>();
        areWeFading = true;
        fader.InitiateFadeIn(init.GetComponent<CanvasGroup>(), init.GetComponent<Image>(), col, fadeInDuration);
    }

    public static void FadeOut(string scene, Color col, float fadeOutDuration)
    {
        if (areWeFading)
        {
            Debug.Log("Already Fading");
            return;
        }

        var init = new GameObject("Fader", typeof(Canvas), typeof(CanvasGroup), typeof(Image), typeof(Fader));
        init.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        var fader = init.GetComponent<Fader>();
        areWeFading = true;
        fader.InitiateFadeOut(init.GetComponent<CanvasGroup>(), init.GetComponent<Image>(), scene, col, fadeOutDuration);
    }

    public static void DoneFading()
    {
        areWeFading = false;
    }
}