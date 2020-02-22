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

    //Sf stands for satisfaction of the customer.
    private int sf;
    private readonly int maxSf = 100;
    private int defaultSf = 0;
    private float sfFactor = 0.0f;
    //private int reviewClass = 1;


    //Methods------------------------------------------------------------------------------------------------

    /// <summary>
    /// Set starting Sf as default Sf.
    /// If default Sf >= 0 use max Sf / 2.
    /// </summary>
    private void Start()
    {
        if (defaultSf <= 0)
        {
            defaultSf = maxSf / 2;
        }

        RecoverSf();
    }


    private void Update()
    {
        //Check if satisfaction is full or minium.
        //TODO: Better way to do this? Lambda?
        if (maxSf <= sf || sf <= 0)
        {
            Review();
            RecoverSf();
        }
    }


    /// <summary>
    /// Get value from satisfaction gain value according to the given value and
    /// current satisfaction factor, convert it to int and sum it to satisfaction.
    /// </summary>
    /// <param name="value">Value that determines the value to be added to satisfaction.</param>
    public void SfGain(int value)
    {
        float f = SfGainValue(value);
        sf += Convert.ToInt32(f);
    }


    /// <summary>
    /// Returns value according to the given value and current satisfaction factor.
    /// 
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
                return value * (1 + sfFactor);
            }
            else
            {
                //POSITIVE value and NEGATIVE satisfaction factor.
                return value / (1 + Mathf.Abs(sfFactor));
            }
        }
        else
        {
            if (0 < sfFactor)
            {
                //NEGATIVE value and POSITIVE satisfaction factor.
                return value / (1 + sfFactor);
            }
            else
            {
                //NEGATIVE value and NEGATIVE satisfaction factor.
                return value * (1 + Mathf.Abs(sfFactor));
            }
        }
    }


    /// <summary>
    /// Sums given positive or negative value to satisfaction factor.
    /// </summary>
    /// <param name="value">Value to be summed to satisfaction factor.</param>
    public void SfFactorGain(float value)
    {
        sfFactor += value;
    }


    /// <summary>
    /// Set satisfaction factor as default satisfaction factor.
    /// </summary>
    public void RecoverSf()
    {
        sf = defaultSf;
    }


    //Gives review. (According to review class?)
    public void Review()
    {
        //TODO Affect game manager or something like that.
    }
}
