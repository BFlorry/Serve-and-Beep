using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerNeedDisplay : MonoBehaviour
{
    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private Image needImage;
    [SerializeField]
    private GameObject needCanvas;

    [SerializeField]
    private CustomerNeed customerNeed;

    // Start is called before the first frame update
    void Start()
    {
        needCanvas.SetActive(true);
        Camera camera = Camera.main;
        needCanvas.transform.LookAt(needCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetProgressDisplayValue();
        SetNeedDisplayValue();
    }

    void SetProgressDisplayValue()
    {
        if (customerNeed.CurrentValue >= customerNeed.MaxValue) needCanvas.SetActive(false);
        else progressImage.fillAmount = 1 - (customerNeed.CurrentValue / customerNeed.MaxValue);
    }

    void SetNeedDisplayValue()
    {

    }
}
