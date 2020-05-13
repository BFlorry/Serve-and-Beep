using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDisplayController : MonoBehaviour
{
    [SerializeField]
    GameObject[] stars;

    public void EnableStar(int starAmount)
    {
        stars[starAmount].SetActive(true);
    }

}
