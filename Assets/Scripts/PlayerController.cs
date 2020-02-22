using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 1f;

    [SerializeField]
    private float rotateSpeed = 1f;

    private float playerSpeedStore;

    AudioListener audioListener;

    private Vector2 moveAxis;

    private bool frozen = false;

    Quaternion turnRotation;

    void Start()
    {
        playerSpeedStore = playerSpeed;
        audioListener = this.GetComponentInChildren<AudioListener>();
        turnRotation = this.transform.rotation;
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

    void PlayerMovementNew()
    {
        Vector3 targetVelocity = new Vector3(moveAxis.x, 0, moveAxis.y);
        GetComponent<Rigidbody>().velocity = targetVelocity * playerSpeed;
        GetComponent<Rigidbody>().velocity = targetVelocity * playerSpeed;

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
