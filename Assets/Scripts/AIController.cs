using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Enums.CustomerEnums;

/// <summary>
/// Class for controlling AI character.
/// </summary>
public class AIController : MonoBehaviour
{
    //Fields------------------------------------------------------------------------

    //Array of area bounds, where an NPC can move.
    private Bounds[] areaBounds = null;
    private Vector3[] points; = null;

    private int areaId = 0;
    private int pointGroupId = 0;

    private IEnumerator waitAfterMove;

    [SerializeField]
    private float
        walkRadius = 7.0f,
        destinationReachedTreshold = 0.01f,
        minWaitTimeAtDest = 2,
        maxWaitTimeAtDest = 10;
        
    [SerializeField]
    private NavMeshAgent agent = null;

    private bool moving = false;


    //Methods------------------------------------------------------------------------

    /// <summary>
    /// Finds a GameObject named CustomerAreas and gets its array of GameObjects.
    /// Then creates an array of bounds from the GameObjects' MeshRenderers.
    /// </summary>
    private void Start()
    {
        GameObject customerAreas = GameObject.Find("CustomerAreas");
        GameObject[] areaObjects = customerAreas.GetComponent<CustomerDestinations>().Destinations;
        areaBounds = new Bounds[areaObjects.Length];
        for (int i = 0; i < areaBounds.Length; i++)
        {
            areaBounds[i] = areaObjects[i].GetComponent<MeshRenderer>().bounds;
        }

        GameObject customerPoints = GameObject.Find("CustomerPoints");
        GameObject[] pointGroups = customerAreas.GetComponent<CustomerDestinations>().Destinations;
        GameObject[] pointObjects = pointGroups[pointGroupId].GetComponent<CustomerDestinations>().Destinations;
        points = new Vector3[pointObjects.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = pointObjects[i].transform.position;
        }
    }

    void Update()
    {
        MoveRandomly();
        //SetAreaIdWithKeyboard();
    }


    public void MoveToNeedLocation(CustomerNeed custNeed)
    {
        agent.isStopped = false;

        this.areaId = (int)custNeed.Area;
        this.pointGroupId = (int)custNeed.Area;

        if (areaId > 0)
        {
            MoveRandomly();
            moving = false;
        }
        else if (pointGroupId > 0)
        {
            MoveToPoint()
        }


    }



    public void StopMovement()
    {
        agent.isStopped = true;
    }


    /// <summary>
    /// If moving and if close enough to target, set waiting.
    /// If not moving and not waiting, move to new location.
    /// If AIController has valid area id set, move inside that area.
    /// Otherwise move randomly inside whole NavMesh.
    /// </summary>
    private void MoveRandomly()
    {
        if (moving)
        {
            if (agent.remainingDistance < destinationReachedTreshold)
            {
                moving = false;
                float waitTime = Random.Range(minWaitTimeAtDest, maxWaitTimeAtDest);
                Debug.Log("Wait time: " + waitTime);
                waitAfterMove = WaitAfterMove(waitTime);
                StartCoroutine(waitAfterMove);
            }
        }

        else if (waitAfterMove == null)
        {
            //If AIController has valid area id, get random point inside area.
            if (0 < areaId)
            {
                Bounds bounds = areaBounds[areaId-1];
                Vector3 point = RandomPointInObject(bounds);
                MoveToPoint(point);
            }
            //If AIController does not have valid area id, get random point inside whole NavMesh.
            else
            {
                MoveToPoint(RandomPoint(Vector3.zero, walkRadius)); //could be (transform.position, walkRadius)
            }
            moving = true;
        }
        
    }


    /// <summary>
    /// Waits given time (seconds) and sets wait after move to null.
    /// </summary>
    /// <param name="time">Time to wait as seconds.</param>
    /// <returns>IEnumerator stuff?</returns>
    private IEnumerator WaitAfterMove(float time)
    {
        yield return new WaitForSeconds(time);
        waitAfterMove = null;
    }


    /// <summary>
    /// Sets NavMeshAgent's movement destination as given point.
    /// </summary>
    /// <param name="point">Target point for movement.</param>
    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
        Debug.DrawRay(point, Vector3.up * 5, Color.blue, 10.0f);
        Debug.Log("Target position: " + point);
    }


    /// <summary>
    /// Returns random point in NavMesh according to given center point and range.
    /// </summary>
    /// <param name="center">Center point of the radius.</param>
    /// <param name="range">Sphere shaped radius. "center" is origo.</param>
    /// <returns>Random point in NavMesh.</returns>
    public Vector3 RandomPoint(Vector3 center, float range)
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        randomDirection += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;
    }


    /// <summary>
    /// Returns random point on NavMesh within given object.
    /// </summary>
    /// <param name="area">The GameObject in which the returned point will be.</param>
    /// <returns>A random point on NavMesh within given object.</returns>
    public Vector3 RandomPointInObject(Bounds bounds)
    {
        Vector3 point = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z));
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, float.MaxValue, 1);
        return hit.position;
    }

}
