using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Customer class that contains customer's current satisfaction,
/// satisfaction factor and methods for modifying them.
/// </summary>
public class Customer : MonoBehaviour
{
    //Fields--------------------------------------------------------------------------------------------------

    [Header ("Customer stats")]

    ///<summary>Maxium satisfaction of the customer.</summary>
    [SerializeField]
    protected int sfMax = 100;

    ///<summary>Current satisfaction of the customer. 0 converts to default at start.</summary>
    [SerializeField]
    protected int sf = 0;

    ///<summary>Default satisfaction of the customer. 0 converts to default at start.</summary>
    [SerializeField]
    protected int sfDefault = 0;

    ///<summary>Satisfaction factor of the customer. "Mood"</summary>
    [SerializeField]
    protected float sfFactor = 0.0f;


    //Methods-------------------------------------------------------------------------------------------------

    /// <summary>
    /// Set starting Sf as default Sf.
    /// If variables' values are less or equal to zero
    /// or bigger than maxium satisfaction,
    /// set the value as half of the maxium satisfaction.
    /// </summary>
    private void Start()
    {
        //If sfDefault is not valid, set it as sfMax / 2.
        if (sfDefault <= 0 || sfMax < sfDefault)
        {
            sfDefault = sfMax / 2;
        }

        //If current satisfaction is not valid, set it as default satisfaction.
        if (sf <= 0 || sfMax < sf)
        {
            sf = sfDefault;
        }
    }


    /// <summary>
    /// Get value from satisfaction gain value according to the given value and
    /// current satisfaction factor, convert it to int and sum it to satisfaction.
    /// Check if satisfaction is full or empty after summing and act accordingly.
    /// </summary>
    /// <param name="value">Value that determines the value to be added to satisfaction.</param>
    public void SfGain(int value)
    {
        float f = SfGainValue(value);
        sf += Convert.ToInt32(f);

        //Check if satisfaction is empty.
        if (sf <= 0)
        {
            ReviewNeg();
            SfRecoveryEmpty();
        }

        //Check if satisfaction is full.
        if (sfMax <= sf)
        {
            ReviewPos();
            SfRecoveryFull();
        }
    }


    /// <summary>
    /// Recovers emptied satisfaction to half of maxium satisfaction.
    /// </summary>
    public void SfRecoveryEmpty()
    {
        sf = sf + sfMax / 2;
    }


    /// <summary>  
    /// Recovers filled satisfaction to half of maxium satisfaction.
    /// </summary> 
    public void SfRecoveryFull()
    {
        sf = (sf - sfMax) + sfMax / 2;
    }


    /// <summary>
    /// Returns value according to the given value and current satisfaction factor:
    /// Positivity/negativity of the given value with positivity/negativity of
    /// the current satisfaction factor determine which mathematical function
    /// is used to convert the given value to the value to be returned.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    public float SfGainValue(int value)
    {
        if (0 < value)
        {
            if (0 < sfFactor)
            {
                //POSITIVE value and POSITIVE satisfaction factor.
                return PosValPosFac(value);
            }
            else
            {
                //POSITIVE value and NEGATIVE satisfaction factor.
                return PosValNegFac(value);
            }
        }
        else
        {
            if (0 < sfFactor)
            {
                //NEGATIVE value and POSITIVE satisfaction factor.
                return NegValPosFac(value);
            }
            else
            {
                //NEGATIVE value and NEGATIVE satisfaction factor.
                return NegValNegFac(value);
            }
        }
    }


    /// <summary>
    /// POSITIVE value and POSITIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    public float PosValPosFac(int value)
    {
        return value * (1 + sfFactor);
    }

    /// <summary>
    /// POSITIVE value and NEGATIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    public float PosValNegFac(int value)
    {
        return value / (1 + Mathf.Abs(sfFactor));
    }

    /// <summary>
    /// NEGATIVE value and POSITIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    public float NegValPosFac(int value)
    {
        return value / (1 + sfFactor);
    }

    /// <summary>
    /// NEGATIVE value and NEGATIVE satisfaction factor.
    /// Converts given value according to the mathematical
    /// function and returns it.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>Converted value.</returns>
    public float NegValNegFac(int value)
    {
        return value * (1 + Mathf.Abs(sfFactor));
    }


    /// <summary>
    /// Sums given positive or negative value to satisfaction factor.
    /// </summary>
    /// <param name="value">Value to be summed to satisfaction factor.</param>
    public void SfFactorGain(float value)
    {
        sfFactor += value;
    }


    //Gives positive review. (According to review class?)
    public void ReviewPos()
    {
        //TODO Affect game manager or something like that.
    }

    //Gives negative review. (According to review class?)
    public void ReviewNeg()
    {
        //TODO Affect game manager or something like that.
    }
}
