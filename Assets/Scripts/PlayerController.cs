﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 1f;

    [SerializeField]
    private float rotateSpeed = 1f;

    [SerializeField]
    private Animator animator;

    private float playerSpeedStore;

    AudioListener audioListener;

    private Vector2 moveAxis;

    private bool frozen = false;

    Quaternion turnRotation;

    PickupController pickupController;

    PlayerInteractionController playerInteractionController;

    void Start()
    {
        playerSpeedStore = playerSpeed;
        audioListener = this.GetComponentInChildren<AudioListener>();
        turnRotation = this.transform.rotation;
        this.TryGetComponent(out pickupController);
        this.TryGetComponent(out playerInteractionController);
    }

    void OnDeviceLost(PlayerInput pi)
    {
        pi.DeactivateInput();
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        PlayerMovementNew();
    }

    private void Update()
    {
        animator.SetFloat("Velocity", new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z).magnitude);
        animator.SetBool("Carrying", GetComponent<PickupController>().Carrying);
    }

    void PlayerMovementNew()
    {
        Vector3 targetVelocity = new Vector3(moveAxis.x, 0, moveAxis.y);
        Vector3 fallingVelocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
        GetComponent<Rigidbody>().velocity = fallingVelocity + (targetVelocity * playerSpeed);

        if(targetVelocity.magnitude > 0 && !frozen) {
            //Rotate player
            turnRotation = Quaternion.LookRotation(targetVelocity);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, Time.deltaTime * rotateSpeed);
    }

    public void OnMovement(InputValue value)
    {
        moveAxis = value.Get<Vector2>();
    }

    void OnPause()
    {
        FindObjectOfType<PauseManager>().TogglePause();
    }

    /// <summary>
    /// When pressing interact-button, first try interacting and if an interact didn't happen, perform a pickup
    /// </summary>
    void OnInteract()
    {
        bool interacted = playerInteractionController.Interact();
        if (interacted == false) pickupController.Pickup();
    }

    public void TogglePlayerFreeze()
    {
        if (!frozen)
        {
            frozen = true;
            playerSpeed = 0;
        }
        else
        {
            frozen = false;
            playerSpeed = playerSpeedStore;
        }
    }
    

}
