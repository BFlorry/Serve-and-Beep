using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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

    [SerializeField]
    private float throwHoldTime = 2.0f;
    private float pickupButtonHeld = float.MaxValue;

    [SerializeField]
    private GameObject throwChargedParticle;

    [SerializeField]
    private AudioClip throwChargedSfx;

private void OnEnable()
    {
        PauseManager.OnPause += EnableMenuControls;
        PauseManager.OnResume += EnablePlayerControls;
        StageOverManager.OnStageOver += EnableMenuControls;
        GameStateController.OnSceneRestart += EnablePlayerControls;
    }

    private void OnDisable()
    {
        PauseManager.OnPause -= EnableMenuControls;
        PauseManager.OnResume -= EnablePlayerControls;
        StageOverManager.OnStageOver -= EnableMenuControls;
        GameStateController.OnSceneRestart -= EnablePlayerControls;
    }

    void Start()
    {
        playerSpeedStore = playerSpeed;
        audioListener = this.GetComponentInChildren<AudioListener>();
        rigidbody = GetComponent<Rigidbody>();
        turnRotation = this.transform.rotation;
        this.TryGetComponent(out pickupController);
        this.TryGetComponent(out playerInteractionController);
        sfxManager = GetComponent<PlayerSfxManager>();
        ForceCheckCamera();
        EnablePlayerControls();
    }

    private void EnablePlayerControls()
    {
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    private void EnableMenuControls()
    {
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        this.GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        this.GetComponent<PlayerInput>().camera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    void OnDeviceLost(PlayerInput pi)
    {
        pi.DeactivateInput();
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        animator.SetFloat("Velocity", new Vector2(rigidbody.velocity.x, rigidbody.velocity.z).magnitude);
        animator.SetBool("Carrying", GetComponent<PickupController>().Carrying);
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector3(moveAxis.x, 0, moveAxis.y);
        Vector3 fallingVelocity = new Vector3(0, rigidbody.velocity.y, 0);
        rigidbody.velocity = fallingVelocity + (targetVelocity * playerSpeed);

        if (targetVelocity.magnitude > 0 && !frozen)
        {
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
                Instantiate(dashParticle, this.transform.position, Quaternion.identity);
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
        playerInteractionController.Interact();
    }

    void OnPickupDown()
    {
        pickupButtonHeld = Time.time + throwHoldTime;
        StartCoroutine(WaitForThrowCharge());
    }

    void OnPickupUp()
    {
        StopAllCoroutines();
        if (Time.time >= pickupButtonHeld)
        {
            bool throwSuccess = pickupController.Throw();
            if (!throwSuccess) Pickup();
        }
        else
        {
            bool didPickupSpawnItem = playerInteractionController.InteractWithItemSpawner();
            if(!didPickupSpawnItem)
                pickupController.TryPickup();
        }
    }

    void Pickup()
    {
        bool didPickupSpawnItem = playerInteractionController.InteractWithItemSpawner();
        if (!didPickupSpawnItem)
            pickupController.TryPickup();
    }

    private IEnumerator WaitForThrowCharge()
    {
        yield return new WaitForSeconds(throwHoldTime);
        if (Time.time >= pickupButtonHeld)
        {
            Instantiate(throwChargedParticle, this.transform.position + Vector3.up * 1.5f, Quaternion.identity);
            sfxManager.PlayRandomized(throwChargedSfx);
        }
    }

    void OnPlayerSoundDown()
    {
        int randInt = Random.Range(0, beepSfx.Length);
        sfxManager.PlaySingleStoppable(beepSfx[randInt]);
    }

    void OnPlayerSoundUp()
    {
        sfxManager.StopSingleStoppable();
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

    public void ForceCheckCamera()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.camera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        if (playerInput.camera == null) playerInput.camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

}
