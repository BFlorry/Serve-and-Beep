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
        SetPlayerMaterial();
        if (playerIndex == -1)
            StartCoroutine(LateCheckPlayerIndex());
    }

    private IEnumerator LateCheckPlayerIndex()
    {
        yield return new WaitForSeconds(0.5f);
        SetPlayerMaterial();
    }

    private void SetPlayerMaterial()
    {
        // If index not set, use random material and color
        if (playerIndex == -1)
        {
            int randInt = Random.Range(0, materials.Length);
            GetComponentInChildren<Renderer>().material = materials[randInt];
            Color randColor = new Color(
            Random.Range(0.7f, 1f),
            Random.Range(0.7f, 1f),
            Random.Range(0.7f, 1f)
            );
            GetComponentInChildren<Renderer>().material.color = randColor;
            StartCoroutine(LateCheckPlayerIndex());
        }
        else
        {
            GetComponentInChildren<Renderer>().material = materials[playerIndex];
        }
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
        GetComponentInChildren<Renderer>().material = materials[playerIndex];
    }
}
