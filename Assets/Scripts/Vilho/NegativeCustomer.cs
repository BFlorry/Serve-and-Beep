using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mick the Prick. Customer that favours low satisfaction.
/// Define fields in prefab.
/// Example: sfMax = 150, sf = 50, sfDefault = 50, sfFactor = -0.5f
/// </summary>
public class NegativeCustomer : Customer
{
    //Functions-------------------------------------------------------------------------------------

    /// <summary>
    /// NEGATIVE value and POSITIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    new private float NegValPosFac(int value)
    {
        return (value / (1 + sfFactor)) - 5;
    }

    /// <summary>
    /// NEGATIVE value and NEGATIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    new private float NegValNegFac(int value)
    {
        return (value * (1 + Mathf.Abs(sfFactor))) - 10;
    }
}
