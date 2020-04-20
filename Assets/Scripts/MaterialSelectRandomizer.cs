using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSelectRandomizer : MonoBehaviour
{
    [SerializeField]
    Material[] materials;
    // Start is called before the first frame update
    void Awake()
    {
        int randInt = Random.Range(0, materials.Length);
        GetComponent<Renderer>().material = materials[randInt];
        Color randColor = new Color(
        Random.Range(0.7f, 1f),
        Random.Range(0.7f, 1f),
        Random.Range(0.7f, 1f)
        );
        GetComponent<Renderer>().material.color = randColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
