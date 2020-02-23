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

    void Update()
    {
        MoveRandomly();
    }


    /// <summary>
    /// If moving and if close enough to target, set waiting.
    /// If not moving and not waiting, move to new location.
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
            Move(RandomPoint(Vector3.zero, walkRadius)); //could be (transform.position, walkRadius)
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
}
