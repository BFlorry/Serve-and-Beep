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

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        needCanvas.SetActive(true);
        camera = Camera.main;
        needCanvas.transform.LookAt(needCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        if(needCanvas.activeSelf) needCanvas.transform.LookAt(needCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        SetProgressDisplayValue();
        SetNeedDisplayValue();
    }

    void SetProgressDisplayValue()
    {
        if (customerNeed.CurrentValue >= customerNeed.MaxValue || customerNeed.CurrentValue <= customerNeed.DefaultValue) needCanvas.SetActive(false);
        else
        {
            progressImage.fillAmount = 1 - (customerNeed.CurrentValue / customerNeed.MaxValue);
            needCanvas.SetActive(true);
        }
    }

    void SetNeedDisplayValue()
    {

    }
}
