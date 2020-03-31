using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavManager : MonoBehaviour
{
    /// <summary>
    /// Returns random point on NavMesh within given object.
    /// </summary>
    /// <param name="area">The GameObject in which the returned point will be.</param>
    /// <returns>A random point on NavMesh within given object.</returns>
    public Vector3 GetRandomPointInArea(GameObject area)
    {
        Bounds bounds = area.GetComponent<MeshRenderer>().bounds;
        Vector3 point = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z));
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, float.MaxValue, 1);
        return hit.position;
    }


    /// <summary>
    /// Returns random point in NavMesh according to given center point and range.
    /// </summary>
    /// <param name="center">Center point of the radius.</param>
    /// <param name="range">Sphere shaped radius. "center" is origo.</param>
    /// <returns>Random point in NavMesh.</returns>
    public Vector3 GetRandomPointInMap(Vector3 center, float range)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;

        randomDirection += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, range, 1);

        return hit.position;
    }


    public CustomerPoint GetRandomFreePoint(GameObject pointGroupObj)
    {
        CustomerPointGroup pointGroup = pointGroupObj.GetComponent<CustomerPointGroup>();
        return pointGroup.GetRandomFreePoint();
    }
}
