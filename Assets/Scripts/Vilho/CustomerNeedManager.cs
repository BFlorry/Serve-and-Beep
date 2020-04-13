﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNeedManager : MonoBehaviour
{
    public CustomerNeed[] Needs { get; private set; }

    [SerializeField]
    private CustomerNeed exitNeed;

    private void Awake()
    {
        Needs = FindObjectsOfType<CustomerNeed>();
        List<CustomerNeed> listNeeds = new List<CustomerNeed>(Needs);
        exitNeed = listNeeds.Find(x => x.gameObject.name == "ExitNeed");

        //TODO: Completely change Needs to a list for easier handling
        listNeeds.Remove(exitNeed);
        Needs = listNeeds.ToArray();
    }

    public CustomerNeed GetRandomNeed()
    {
        return Needs[Random.Range(0, Needs.Length)];
    }

    public CustomerNeed GetExitNeed()
    {
        return exitNeed;
    }
}
