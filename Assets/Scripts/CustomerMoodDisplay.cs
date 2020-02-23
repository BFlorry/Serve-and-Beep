using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMoodDisplay : MonoBehaviour
{
    [SerializeField]
    private Image moodImage;
    //[SerializeField]
    //private GameObject moodCanvas;

    [SerializeField]
    private Customer customer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        customer.SfGain(1);
        SetMoodDisplayValue();
    }

    void SetMoodDisplayValue()
    {
        moodImage.fillAmount = (float)customer.Sf / (float)customer.SfMax;
    }
}
