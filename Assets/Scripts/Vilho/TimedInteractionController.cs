using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedInteractionController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float interactionTime = 2.0f;

    private float interactionTimeLeft;
    //private bool interactable = true;

    bool isInteracting = false;

    PlayerController playerController;

    ProgressBarController progressBar;

    Canvas canvas;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        progressBar = GetComponentInChildren<ProgressBarController>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        interactionTimeLeft = interactionTime;
    }

    void Update()
    {
        if (isInteracting)
        {
            interactionTimeLeft -= Time.deltaTime;
            Debug.Log("Interaction. Interaction time left: " + interactionTimeLeft);
            progressBar.SetProgressBarValue(1 - interactionTimeLeft / interactionTime);
            if (interactionTimeLeft <= 0.0f)
            {
                InteractionEvent();
            }
        }
    }


    /// <summary>
    /// Reduces the completion time in seconds since the last frame from interactionTimeLeft.
    /// Calls InteractionEvent when interactionTimeLeft reaches zero.
    /// </summary>
    public void Interact()
    {
        playerController.TogglePlayerFreeze();

        if (!isInteracting)
        {
            isInteracting = true;
            canvas.gameObject.SetActive(true);
        }
        else
        {
            DeInteract();
        }
    }

    private void DeInteract()
    {
        isInteracting = false;
        canvas.gameObject.SetActive(false);
    }

    public void InteractionEvent()
    {
        //interactable = false;
        playerController.TogglePlayerFreeze();
        DeInteract();
        interactionTimeLeft = interactionTime;
        Debug.Log("Counter interaction event.");
        Destroy(gameObject);
    }
    
}
