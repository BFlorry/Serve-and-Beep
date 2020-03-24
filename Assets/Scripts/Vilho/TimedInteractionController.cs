using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedInteractionController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float interactionTime = 2.0f;

    [SerializeField]
    AudioClip interactSfx;

    private float interactionTimeLeft;

    bool isInteracting = false;

    PlayerController playerController;

    [SerializeField]
    private Image progressBarImage;
    [SerializeField]
    private GameObject progressBarCanvas;

    void Awake()
    {
        Camera camera = Camera.main;
        progressBarCanvas.transform.LookAt(progressBarCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        interactionTimeLeft = interactionTime;
    }

    void Update()
    {
        if (isInteracting)
        {
            interactionTimeLeft -= Time.deltaTime;
            Debug.Log("Interaction. Interaction time left: " + interactionTimeLeft);
            SetProgressBarValue(1 - interactionTimeLeft / interactionTime);
            if (interactionTimeLeft <= 0.0f)
            {
                InteractionEvent();
            }
        }
        if (interactionTimeLeft >= interactionTime)
        {
            progressBarCanvas.SetActive(false);
        }
    }


    /// <summary>
    /// Reduces the completion time in seconds since the last frame from interactionTimeLeft.
    /// Calls InteractionEvent when interactionTimeLeft reaches zero.
    /// </summary>
    public void Interact(GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
        playerController.TogglePlayerFreeze();

        if (!isInteracting)
        {
            progressBarCanvas.SetActive(true);
            isInteracting = true;
        }
        else
        {
            DeInteract();
        }
    }

    private void DeInteract()
    {
        isInteracting = false;
    }

    void InteractionEvent()
    {
        playerController.TogglePlayerFreeze();
        DeInteract();
        progressBarCanvas.SetActive(false);
        interactionTimeLeft = interactionTime;
        Debug.Log("Counter interaction event.");
        playerController.GetComponent<PlayerSfxManager>().PlaySingle(interactSfx);
        InteractionAction();
    }

    public virtual void InteractionAction()
    {
        Destroy(gameObject);
    }

    void SetProgressBarValue(float value)
    {
        progressBarImage.fillAmount = value;
    }

}
