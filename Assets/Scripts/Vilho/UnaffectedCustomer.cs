using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A customer that starts with default satisfaction after giving review.
/// Define fields in prefab.
/// Example: sfDefault = 0;
/// </summary>
public class UnaffectedCustomer : Customer
{
    [SerializeField]
    new private int sfDefault = 0;

    //Functions-------------------------------------------------------------------------------------
    
    /// <summary>
    /// Sets satisfaction as default satisfaction.
    /// </summary>
    new public void SfRecoveryEmpty()
    {
        sf = sfDefault;
    }

    /// <summary>
    /// Sets satisfaction as default satisfaction.
    /// </summary>
    new public void SfRecoveryFull()
    {
        sf = sfDefault;
    }
}
