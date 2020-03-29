using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    private AudioSource source;
    [SerializeField]
    private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        Initiate.FadeIn(Color.black, 4.0f);

        source = gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(clip);
        StartCoroutine(WaitUntilFadeOut());
        //StartCoroutine(WaitForSound());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            OnSkipSplash();
        }
    }

    IEnumerator WaitUntilFadeOut()
    {
        yield return new WaitForSeconds(5);

        Initiate.FadeOut("Main_Menu", Color.black, 3.0f);
    }

    /*IEnumerator WaitForSound()
    {
        //Wait Until Sound has finished playing
        while (source.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadSceneAsync("Main_Menu");
    }*/

    void OnSkipSplash()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
