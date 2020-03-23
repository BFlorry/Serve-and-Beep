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

    private CustomerPoint curCustomerPoint = null;

    private AIManager aiManager = null;
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


    private void SetAIManager()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        aiManager = gameManager.GetComponent<AIManager>();
    }


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
            aiManager.GetRandomPointInMap(this.gameObject.transform.position, walkRadius);
        }
        else MoveToPoint(aiManager.GetRandomPointInArea(area));
    }


    public void MoveToPointGroup(PointGroupEnum pointGroup)
    {
        this.agent.isStopped = false;

        CustomerPoint customerPoint = aiManager.GetRandomFreePoint(pointGroup);
        if (curCustomerPoint != false)
        {
            curCustomerPoint.IsOccupied = false;
        }
        curCustomerPoint = customerPoint;
        this.curArea = AreaEnum.Empty;

        MoveToPoint(customerPoint.Position);
    }


    public void StopMovement()
    {
        agent.isStopped = true;
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


    /// <summary>
    /// Sets NavMeshAgent's movement destination as given point.
    /// </summary>
    /// <param name="point">Target point for movement.</param>
    private void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
        Debug.DrawRay(point, Vector3.up * 5, Color.blue, 10.0f);
        Debug.Log("Target position: " + point);
    }
}
