using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOxygen : MonoBehaviour
{
    public Text text;
    OxygenController oxy;
    // Start is called before the first frame update
    void Start()
    {
        //text = this.GetComponentInChildren<Text>();
        oxy = this.GetComponent<OxygenController>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = oxy.Oxygen.ToString();
    }
}
