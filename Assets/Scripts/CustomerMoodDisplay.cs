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
        
        //TODO: These should probably be called only when the value is changed.
        SetMoodDisplayValue();
        SetArrowDisplayValue(customer.SfFactor);
    }

    void SetMoodDisplayValue()
    {
        moodImage.fillAmount = (float)customer.Sf / (float)customer.SfMax;
    }

    //If given value is positive, set arrow display positive,
    //else set negative.
    void SetArrowDisplayValue(float value)
    {
        if (value >= 0)
        {
            arrowImage.color = Color.green;
            arrowImage.transform.rotation = posRotation;
        }

        else
        {
            arrowImage.color = Color.red;
            arrowImage.transform.rotation = negRotation;
        }
    }
}
