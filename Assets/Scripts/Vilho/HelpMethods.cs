using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class for containing universal help methods.
/// </summary>
public static class HelpMethods
{
    /// <summary>
    /// Compare similiarity of two values with threshold.
    /// https://answers.unity.com/questions/756538/mathfapproximately-with-a-threshold.html
    /// </summary>
    /// <param name="a">first value to be compared</param>
    /// <param name="b">second value to be compared</param>
    /// <param name="threshold">threshold</param>
    /// <returns></returns>
    public static bool Approximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
