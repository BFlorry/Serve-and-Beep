using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    private static Image progressBarImage;


    void Start()
    {
        progressBarImage = GetComponent<Image>();
    }
    
    public void SetProgressBarValue(float value)
    {
        progressBarImage.fillAmount = value;
    }
    
}
