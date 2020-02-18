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
    ProgressBarHideController progressBarHide;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        progressBarHide = playerController.GetComponentInChildren<ProgressBarHideController>();
        //progressBar = progressBarHide.GetComponentInChildren<ProgressBarController>();
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
            progressBarHide.HideProgressBar(false);
            progressBar = progressBarHide.GetComponentInChildren<ProgressBarController>();
        }
        else
        {
            DeInteract();
        }
    }

    private void DeInteract()
    {
        isInteracting = false;
        progressBarHide.HideProgressBar(true);
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
