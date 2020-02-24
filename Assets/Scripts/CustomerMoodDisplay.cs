using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMoodDisplay : MonoBehaviour
{
    [SerializeField]
    private Image moodImage;
    [SerializeField]
    private Image arrowImage;
    [SerializeField]
    private GameObject moodCanvas;

    [SerializeField]
    private Customer customer;

    private Quaternion posRotation;
    private Quaternion negRotation;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        moodCanvas.SetActive(true);
        camera = Camera.main;
        moodCanvas.transform.LookAt(moodCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        posRotation = arrowImage.transform.rotation;
        negRotation = posRotation * Quaternion.Euler(0, 0, 180);
    }

    private void Update()
    {
        moodCanvas.transform.LookAt(moodCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        SetMoodDisplayValue();
        SetArrowDisplayValue();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //customer.SfGain(1);
        //customer.SfFactorGain(-1);
        
    }

    void SetMoodDisplayValue()
    {
        moodImage.fillAmount = (float)customer.Sf / (float)customer.SfMax;
    }

    void SetArrowDisplayValue()
    {
        if (customer.SfFactor >= 0) SetArrowPositive();
        else SetArrowNegative();
    }

    void SetArrowPositive()
    {
        arrowImage.color = Color.green;
        arrowImage.transform.rotation = posRotation; 
    }

    void SetArrowNegative()
    {
        arrowImage.color = Color.red;
        arrowImage.transform.rotation = negRotation;
    }
}
