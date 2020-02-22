using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reo Speedwagon. Customer that favours high satisfaction.
/// Satisfaction can not be negative. Satisfaction recovers always to same value.
/// Define fields in prefab.
/// Example: sfMax = 200, sf = 150, sfDefault = 50
/// </summary>
public class PositiveCustomer : Customer
{
    //Fields----------------------------------------------------------------------------------------

    [SerializeField]
    private readonly int sfRecovery = 130;

    [SerializeField]
    new private float sfFactor = 0.5f;


    //Functions-------------------------------------------------------------------------------------

    /// <summary>
    /// POSITIVE value and POSITIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    new public float PosValPosFac(int value)
    {
        return (value * (1 + sfFactor)) * 3;
    }

    /// <summary>
    /// POSITIVE value and NEGATIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    new public float PosValNegFac(int value)
    {
        return value / (1 + Mathf.Abs(sfFactor));
    }


    /// <summary>
    /// Sums given value to satisfaction factor.
    /// Prevents satisfaction factor from going negative.
    /// </summary>
    /// <param name="value">Value to be added to satisfaction factor.</param>
    new public void SfFactorGain(float value)
    {
        sfFactor += value;
        if (sfFactor < 0)
        {
            sfFactor = 0.0f;
        }
    }


    /// <summary>
    /// Sets satisfaction according to satisfaction recovery value.
    /// </summary>
    new public void SfRecoveryEmpty()
    {
        sf = sfRecovery;
    }

    /// <summary>
    /// Sets satisfaction according to satisfaction recovery value.
    /// </summary>
    new public void SfRecoveryFull()
    {
        sf = sfRecovery;
    }
}
