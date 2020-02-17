using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : MonoBehaviour
{
    [SerializeField]
    private float oxygen = 0;

    [SerializeField]
    private bool isLeaking = false;

    [SerializeField]
    private float oxyLeakingRate = 0.01f;

    public float Oxygen { get => oxygen; set => oxygen = value; }

    // Start is called before the first frame update
    void Start()
    {
        Oxygen = 100f;
    }

    void FixedUpdate()
    {
        if (isLeaking && Oxygen > 0)
        {
            if (Oxygen - oxyLeakingRate < 0) Oxygen = 0;
            else Oxygen -= oxyLeakingRate;
        }
    }

    private void Update()
    {
        if (Oxygen <= 0) Oxygen = 0;
        else if (Oxygen > 100) Oxygen = 100;
    }

}
