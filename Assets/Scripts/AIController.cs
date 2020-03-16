using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for controlling AI character.
/// </summary>
public class AIController : MonoBehaviour
{
    //Fields------------------------------------------------------------------------

    //An area where an NPC can move from another area.
    [SerializeField]
    private GameObject area1;

    //An area where an NPC can move from another area.
    [SerializeField]
    private GameObject area2;

    //An area where an NPC can move from another area.
    [SerializeField]
    private GameObject area3;

    private List<GameObject> areas;

    private int areaId = -1;

    private IEnumerator waitAfterMove;

    [SerializeField]
    private float
        walkRadius = 7.0f,
        destinationReachedTreshold = 0.01f,
        minWaitTimeAtDest = 2,
        maxWaitTimeAtDest = 10;
        
    [SerializeField]
    private NavMeshAgent agent;

    private bool moving = false;


    //Methods------------------------------------------------------------------------

    private void Start()
    {
        areas.Add(new GameObject());
        areas.Add(area1);
        areas.Add(area2);
        areas.Add(area3);
    }


    void Update()
    {
        MoveRandomly();
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
            if (0 <= areaId)
            {
                GameObject area = areas[areaId];
                Move(RandomPointInObject(area));
            }
            //If AIController does not have valid area id, get random point inside whole NavMesh.
            else
            {
                Move(RandomPoint(Vector3.zero, walkRadius)); //could be (transform.position, walkRadius)
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
    public void Move(Vector3 point)
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
    /// Sets an id of an area where the player will move to.
    /// </summary>
    /// <param name="areaId">Id of an area where the player will move to.</param>
    public void MoveToArea(int areaId)
    {
        this.areaId = areaId;
    }


    /// <summary>
    /// Returns random point on NavMesh within given object.
    /// </summary>
    /// <param name="area">The GameObject in which the returned point will be.</param>
    /// <returns>A random point on NavMesh within given object.</returns>
    public Vector3 RandomPointInObject(GameObject area)
    {
        Bounds bounds = area.GetComponent<Bounds>();
        Vector3 point = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z));
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, float.MaxValue, 1);
        return hit.position;
    }

}
