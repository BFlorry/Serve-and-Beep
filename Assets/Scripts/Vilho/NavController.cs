using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static HelpMethods;

/// <summary>
/// Class for controlling AI character's movement.
/// </summary>
public class NavController : MonoBehaviour
{
    //Fields------------------------------------------------------------------------

    [SerializeField]
    private readonly float
        walkRadius = 7.0f,
        destReachTreshold = 0.1f,
        minWaitTime = 4f,
        maxWaitTime = 10f,
        destRotSpeed = 0.002f;

    [SerializeField]
    private bool moveRandomlyInAreas = true;

    private NavManager navManager;
    private NavMeshAgent agent;
    private CustomerNeed curNeed;
    private CustomerPoint curPoint;
    private IEnumerator wait;
    private bool moving = false;


    //Methods------------------------------------------------------------------------

    /// <summary>
    /// Finds a GameObject named CustomerAreas and gets its array of GameObjects.
    /// Then creates an array of bounds from the GameObjects' MeshRenderers.
    /// </summary>
    private void Awake()
    {
        this.navManager = FindObjectOfType<NavManager>();
        this.agent = GetComponent<NavMeshAgent>();
    }


    /// <summary>
    /// Checks if customer has reached destination and if yes, sets
    /// the customer waiting. Also handles random customer movement.
    /// </summary>
    void Update()
    {
        if (moving == true)
        {
            if (agent.remainingDistance < destReachTreshold)
            {
                moving = false;
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                wait = Wait(waitTime);
                StartCoroutine(wait);

                if (curPoint != null)
                {
                    StartCoroutine(RotateTo(curPoint.gameObject.transform));
                }

            }
        }
        else if (wait == null && moveRandomlyInAreas && curPoint == null)
        {
            MoveToArea(curNeed.Area);
        }
    }


    private IEnumerator RotateTo(Transform destTransform)
    {
        agent.updateRotation = false;
        while (Approximately(this.transform.rotation.y, destTransform.rotation.y, 0.1f) == false)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, destTransform.rotation, destRotSpeed * Time.time);
            yield return null;
        }
        agent.updateRotation = true;
        yield break;
    }

    /// <summary>
    /// If customer has active point, move there.
    /// Else move to area, empty or not.
    /// </summary>
    /// <param name="customerNeed">customer need</param>
    public void MoveToNeedDestination(CustomerNeed customerNeed)
    {
        curNeed = customerNeed;

        if (customerNeed.PointGroup != null)
        {
            MoveToPointGroup(customerNeed.PointGroup);
        }
        else
        {
            MoveToArea(customerNeed.Area);
        }
    }


    /// <summary>
    /// Makes sure that possible customer point is set unoccupied
    /// and sets current customer point to null. Sets current area.
    /// If area is empty, moves to random point on map in walk radius.
    /// Else moves to random point on given area.
    /// </summary>
    /// <param name="area">are to move</param>
    public void MoveToArea(GameObject area)
    {
        if (curPoint != null)
        {
            curPoint.IsOccupied = false;
            curPoint = null;
        }

        StopAllCoroutines();
        this.agent.isStopped = false;
        this.agent.updateRotation = true;
        this.moving = true;

        if (area == null)
        {
            Vector3 position = this.gameObject.transform.position;
            MoveToPoint(navManager.GetRandomPointInMap(position, walkRadius));
        }
        else
        {
            MoveToPoint(navManager.GetRandomPointInArea(area));
        }
    }


    /// <summary>
    /// Gets random free point from according to given point group.
    /// Sets possible previous current customer point unoccupied and
    /// sets new point as current customer point. Sets current area empty.
    /// Sets movement towards the new point.
    /// </summary>
    /// <param name="pointGroup">point group that determines possible destinations</param>
    public void MoveToPointGroup(GameObject pointGroup)
    {
        CustomerPoint customerPoint = navManager.GetRandomFreePoint(pointGroup);

        if (customerPoint != null)
        {
            if (curPoint != null)
            {
                curPoint.IsOccupied = false;
            }
                this.curPoint = customerPoint;
                this.agent.isStopped = false;
                MoveToPoint(customerPoint.Position);
        }
        else
        {
            MoveToArea(curNeed.Area);
        }

        StopAllCoroutines();
        this.agent.isStopped = false;
        this.agent.updateRotation = true;
        this.moving = true;
    }


    /// <summary>
    /// Sets NavMeshAgent's movement destination as given point.
    /// </summary>
    /// <param name="point">target point for movement</param>
    private void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
        Debug.DrawRay(point, Vector3.up * 5, Color.blue, 10.0f);
        Debug.Log("Target position: " + point);
    }


    /// <summary>
    /// Sets NavMesh Agent stopped.
    /// </summary>
    public void StopMovement()
    {
        this.agent.isStopped = true;
    }


    /// <summary>
    /// Waits given time (seconds) and sets wait after move to null.
    /// </summary>
    /// <param name="time">Time to wait as seconds.</param>
    /// <returns>IEnumerator stuff?</returns>
    private IEnumerator Wait(float time)
    {
        Debug.Log("Wait " + time + " seconds after movement.");
        yield return new WaitForSeconds(time);
        wait = null;
        StopAllCoroutines();
        this.agent.isStopped = false;
        this.agent.updateRotation = true;
        this.moving = true;

        if (curNeed.PointGroup != null && curPoint == null)
        {
            MoveToPointGroup(curNeed.PointGroup);
        }
    }
}
