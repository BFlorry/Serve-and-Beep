using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarHideController : MonoBehaviour
{
    GameObject progressBar;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GameObject.FindWithTag("ProgressBar");
        HideProgressBar(true);
    }

    public void HideProgressBar(bool hide)
    {
        progressBar.SetActive(!hide);
    }
}
