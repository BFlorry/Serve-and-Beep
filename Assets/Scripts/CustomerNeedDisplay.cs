using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerNeedDisplay : MonoBehaviour
{
    [SerializeField]
    private Image progressImage;

    [SerializeField]
    private GameObject needCanvas;
    [SerializeField]
    private GameObject moodCanvas;

    private Image needImg;
    private CustomerNeedController needController = null;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        needController = this.GetComponent<CustomerNeedController>();
        needCanvas.SetActive(true);
        camera = Camera.main;
        needCanvas.transform.LookAt(needCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        needImg = GetComponentInChildren<Image>();
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
        if (needController.CurrentValue >= needController.MaxValue || needController.CurrentValue <= needController.DefaultValue) ;
        else
        {
            progressImage.fillAmount = 1 - (needController.CurrentValue / needController.MaxValue);
        }
    }

    void SetNeedDisplayValue()
    {
        
    }

    public void SetNeedImage(Sprite image)
    {
        needImg.sprite = image;
    }


    public void SetNeedCanvasActivity(bool b)
    {
        needCanvas.SetActive(b);
        moodCanvas.SetActive(b);
    }
}
