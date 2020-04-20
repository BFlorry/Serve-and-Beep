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

    [SerializeField]
    private Animator animator;

    // TODO: Remove this when decided on dash style
    //[SerializeField]
    //private float dashForce = 80f;

    [SerializeField]
    private float dashDistance = 3f;

    [SerializeField]
    private float dashCooldown = 0.2f;

    private float nextDash = 0f;

    private float playerSpeedStore;

    AudioListener audioListener;

    private Vector2 moveAxis;
    private bool frozen = false;
    private Quaternion turnRotation;

    private PickupController pickupController;
    private PlayerInteractionController playerInteractionController;

    private Rigidbody rigidbody;
    private PlayerSfxManager sfxManager;

    [SerializeField]
    private AudioClip dashSfx;

    [SerializeField]
    private GameObject dashParticle;

    [SerializeField]
    private AudioClip bumpSfx;

    [SerializeField]
    private AudioClip[] beepSfx;


    void Start()
    {
        playerSpeedStore = playerSpeed;
        audioListener = this.GetComponentInChildren<AudioListener>();
        rigidbody = GetComponent<Rigidbody>();
        turnRotation = this.transform.rotation;
        this.TryGetComponent(out pickupController);
        this.TryGetComponent(out playerInteractionController);
        sfxManager = GetComponent<PlayerSfxManager>();
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
        animator.SetFloat("Velocity", new Vector2(rigidbody.velocity.x, rigidbody.velocity.z).magnitude);
        animator.SetBool("Carrying", GetComponent<PickupController>().Carrying);
    }

    void PlayerMovementNew()
    {
        Vector3 targetVelocity = new Vector3(moveAxis.x, 0, moveAxis.y);
        Vector3 fallingVelocity = new Vector3(0, rigidbody.velocity.y, 0);
        rigidbody.velocity = fallingVelocity + (targetVelocity * playerSpeed);

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

    public void OnDash()
    {
        Vector3 dashVector = new Vector3(moveAxis.x, 0f, moveAxis.y) * dashDistance;
        if (moveAxis.magnitude > 0 && Time.time >= nextDash)
        {
            // Calculate cooldown
            nextDash = Time.time + dashCooldown;

            RaycastHit hit;
            sfxManager.PlayRandomized(dashSfx);
            if (Physics.Raycast(rigidbody.position, dashVector.normalized, out hit, dashVector.magnitude))
            {
                // If hit something, then cause player to fall
                rigidbody.MovePosition(rigidbody.position + dashVector.normalized * hit.distance - dashVector.normalized * 0.8f);
                animator.Play("Bump");
                sfxManager.PlayRandomized(bumpSfx);
            }
            else
            {
                // Didn't hit anything
                Instantiate(dashParticle, this.transform.position, Quaternion.identity);
                // TODO: Remove this when decided on dash style
                //rigidbody.AddForce(new Vector3(moveAxis.x, 0f, moveAxis.y) * dashForce, ForceMode.VelocityChange);
                rigidbody.MovePosition(rigidbody.position + dashVector);
            }
        }
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
        if (interacted == false) pickupController.TryPickup();
    }

    void OnPlayerSound()
    {
        int randInt = Random.Range(0, beepSfx.Length);
        sfxManager.PlaySingle(beepSfx[randInt]);

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
