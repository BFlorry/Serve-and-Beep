using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [SerializeField]
    int maximumNeedsBeforeExit = 3;

    [SerializeField]
    int currentNeed = 0;

    private GameObject exit;

    // Start is called before the first frame update
    void Start()
    {
        exit = FindObjectOfType<CustomerSpawner>().gameObject;
    }

    public void RaiseNeed()
    {
        currentNeed++;
        /*if(currentNeed >= maximumNeedsBeforeExit)
        {
            //TODO: Make customer go towards exit
            customerNeedController.ExitNeed();
        }*/
    }

    public bool TimeToExit()
    {
        if (currentNeed >= maximumNeedsBeforeExit)
        {
            return true;
        }
        else return false;
    }
}
