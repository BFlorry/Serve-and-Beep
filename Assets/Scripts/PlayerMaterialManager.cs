using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMaterialManager : MonoBehaviour
{
    [SerializeField]
    Material[] materials;

    private int playerIndex = -1;

    // Start is called before the first frame update
    void OnEnable()
    {
        // If index not set, use random material and color
        if(playerIndex == -1)
        {
        int randInt = Random.Range(0, materials.Length);
        GetComponentInChildren<Renderer>().material = materials[randInt];
        Color randColor = new Color(
        Random.Range(0.7f, 1f),
        Random.Range(0.7f, 1f),
        Random.Range(0.7f, 1f)
        );
            GetComponentInChildren<Renderer>().material.color = randColor;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
        GetComponentInChildren<Renderer>().material = materials[index];
    }
}
