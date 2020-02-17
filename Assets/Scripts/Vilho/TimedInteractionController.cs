using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedInteractionController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float interactionTime = 2.0f;

    private float interactionTimeLeft;
    //private bool interactable = true;


    void Start()
    {
        interactionTimeLeft = interactionTime;
    }


    /// <summary>
    /// Reduces the completion time in seconds since the last frame from interactionTimeLeft.
    /// Calls InteractionEvent when interactionTimeLeft reaches zero.
    /// </summary>
    public void Interact()
    {
        interactionTimeLeft -= Time.deltaTime;
        if (interactionTimeLeft <= 0.0f) InteractionEvent();
        Debug.Log("Interaction. Interaction time left: " + interactionTimeLeft);
    }

    
    public void InteractionEvent()
    {
        //interactable = false;
        Destroy(gameObject);
        Debug.Log("Counter interaction event.");
    }
}
