using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color randColor = new Color(
        Random.Range(0f, 1f),
        Random.Range(0f, 1f),
        Random.Range(0f, 1f)
        );
        GetComponent<Renderer>().material.color = randColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
