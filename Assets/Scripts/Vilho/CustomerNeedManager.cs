using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNeedManager : MonoBehaviour
{
    public CustomerNeed[] Needs { get; private set; }

    private void Awake()
    {
        Needs = FindObjectsOfType<CustomerNeed>();
    }

    public CustomerNeed GetRandomNeed()
    {
        return Needs[Random.Range(0, Needs.Length)];
    }
}
