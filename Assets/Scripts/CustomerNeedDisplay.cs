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

    private void OnEnable()
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
        if(needCanvas.activeSelf)
        {
            needCanvas.transform.LookAt(needCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }

    }

    public void SetProgressDisplayValue(float curValue, float maxValue, float defaultValue)
    {
        if (curValue >= maxValue || curValue <= defaultValue) ;
        else
        {
            progressImage.fillAmount = 1 - (curValue / maxValue);
        }
    }


    public void SetNeedSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            needImg.sprite = sprite;
        }
        else
        {
            Debug.Log("Need sprite is null and not set.");
        }
    }


    public void SetNeedCanvasActivity(bool b)
    {
        needCanvas.SetActive(b);
        moodCanvas.SetActive(b);
    }
}
