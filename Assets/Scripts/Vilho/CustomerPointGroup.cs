using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that contains customer points.
/// </summary>
public class CustomerPointGroup : MonoBehaviour
{
    //Fields--------------------------------------------------------------------

    [SerializeField]
    private CustomerPoint[] customerPoints = null;


    //Methods-------------------------------------------------------------------

    /// <summary>
    /// Lists unoccupied points and returns one randomly.
    /// </summary>
    /// <returns></returns>
    public CustomerPoint GetRandomFreePoint()
    {
        List<CustomerPoint> freePoints = new List<CustomerPoint>();

        foreach (CustomerPoint point in customerPoints)
        {
            if (point.IsOccupied == false)
            {
                freePoints.Add(point);
            }
        }

        int pointIndex = Random.Range(0, freePoints.Count);
        CustomerPoint customerPoint = freePoints[pointIndex];
        customerPoint.IsOccupied = true;
        return customerPoint;
    }
}
