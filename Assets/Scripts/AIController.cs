using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Enums.CustomerEnums;

/// <summary>
/// Class for controlling AI character's movement.
/// </summary>
public class AIController : MonoBehaviour
{
    //Fields------------------------------------------------------------------------

    [SerializeField]
    private readonly float
        walkRadius = 7.0f,
        destinationReachedTreshold = 2f,
        minWaitTimeAtDest = 2,
        maxWaitTimeAtDest = 10;

    [SerializeField]
    private NavMeshAgent agent = null;

    [SerializeField]
    private bool moveRandomlyInAreas = true;

    private readonly string gameManagerName = "GameManager";
    private AIManager aiManager = null;
    private CustomerPoint curCustomerPoint = null;
    private AreaEnum curArea = AreaEnum.Empty;
    private IEnumerator wait;
    private bool moving = false;


    //Methods------------------------------------------------------------------------

    /// <summary>
    /// Finds a GameObject named CustomerAreas and gets its array of GameObjects.
    /// Then creates an array of bounds from the GameObjects' MeshRenderers.
    /// </summary>
    private void Awake()
    {
        SetAIManager();
    }


    /// <summary>
    /// Checks if customer has reached destination and if yes, sets
    /// the customer waiting. Also handles random customer movement.
    /// </summary>
    void Update()
    {
        if (moving == true)
        {
            if (agent.remainingDistance < destinationReachedTreshold)
            {
                moving = false;
                float waitTime = Random.Range(minWaitTimeAtDest, maxWaitTimeAtDest);
                Debug.Log("Wait time: " + waitTime);
                wait = Wait(waitTime);
                StartCoroutine(wait);
            }
        }
        else if (wait == null && moveRandomlyInAreas && curCustomerPoint == null)
        {
            MoveToArea(curArea);
        }
    }


    /// <summary>
    /// Initializes 
    /// </summary>
    private void SetAIManager()
    {
        GameObject gameManager = GameObject.Find(gameManagerName);
        aiManager = gameManager.GetComponent<AIManager>();
    }


    /// <summary>
    /// If customer has active point, move there.
    /// Else move to area, empty or not.
    /// </summary>
    /// <param name="customerNeed">customer need</param>
    public void MoveToNeedDestination(CustomerNeed customerNeed)
    {
        if (customerNeed.Point != PointGroupEnum.Empty)
        {
            MoveToPointGroup(customerNeed.Point);
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
    public void MoveToArea(AreaEnum area)
    {
        if(curCustomerPoint != null)
        {
            curCustomerPoint.IsOccupied = false;
            curCustomerPoint = null;
        }

        this.agent.isStopped = false;
        this.moving = true;
        this.curArea = area;

        if (area == AreaEnum.Empty)
        {
            Vector3 position = this.gameObject.transform.position;
            MoveToPoint(aiManager.GetRandomPointInMap(position, walkRadius));
        }
        else MoveToPoint(aiManager.GetRandomPointInArea(area));
    }


    /// <summary>
    /// Gets random free point from according to given point group.
    /// Sets possible previous current customer point unoccupied and
    /// sets new point as current customer point. Sets current area empty.
    /// Sets movement towards the new point.
    /// </summary>
    /// <param name="pointGroup">point group that determines possible destinations</param>
    public void MoveToPointGroup(PointGroupEnum pointGroup)
    {
        CustomerPoint customerPoint = aiManager.GetRandomFreePoint(pointGroup);

        if (curCustomerPoint != null)
        {
            curCustomerPoint.IsOccupied = false;
        }

        this.curCustomerPoint = customerPoint;
        this.curArea = AreaEnum.Empty;
        this.agent.isStopped = false;
        MoveToPoint(customerPoint.Position);
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
        yield return new WaitForSeconds(time);
        wait = null;
    }
}
