using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Controls controls;

    [SerializeField]
    private string interactKey;

    List<GameObject> currentCollisions = new List<GameObject>();

    [SerializeField]
    private float playerSpeed = 1f;

    [SerializeField]
    private float maxRayDistance = 1f;

    public Transform carryPosition;

    private float playerSpeedStore;
    private bool interacting;
    private bool isCarrying = false;
    GameObject interactObject;
    private GameObject carryObject;

    AudioListener audioListener;

    private Vector2 moveAxis;

    PickupController pickupController;

    private bool frozen = false;

    public void Awake()
    {

        controls = new Controls();

        PlayerInteractionController piController = GetComponent<PlayerInteractionController>();
        controls.Player.Interact.performed += ctx => piController.TryInteraction();
        controls.Player.Interact.Enable();
        
        controls.Player.Pickup.performed += ctx => GetComponent<PickupController>().Pickup();
        controls.Player.Pickup.Enable();
        controls.Player.Throw.performed += ctx => GetComponent<PickupController>().ThrowObject();
        controls.Player.Throw.Enable();

        controls.Player.Movement.performed += MovementInput;
        controls.Player.Movement.Enable();
    }

    void Start()
    {
        interacting = false;
        isCarrying = false;
        playerSpeedStore = playerSpeed;
        audioListener = this.GetComponentInChildren<AudioListener>();
    }

    void FixedUpdate()
    {
        PlayerMovementNew();


    }

    void PlayerMovement()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed;
    }

    void PlayerMovementNew()
    {
        //Debug.Log(controls.Player.Movement.ReadValue<Vector2>());
        moveAxis = controls.Player.Movement.ReadValue<Vector2>();

        Vector3 targetVelocity = new Vector3(moveAxis.x, 0, moveAxis.y);
        GetComponent<Rigidbody>().velocity = targetVelocity * playerSpeed;
        GetComponent<Rigidbody>().velocity = targetVelocity * playerSpeed;

        if(targetVelocity.magnitude > 0 && !frozen) { 
        //Rotate player
        transform.rotation = Quaternion.LookRotation(targetVelocity);
        }
    }

    void Update()
    {

    }

    public void TogglePlayerFreeze()
    {
        if (!frozen)
        {
            frozen = true;
            playerSpeed = 0;
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            frozen = false;
            playerSpeed = playerSpeedStore;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
    }

    private void MovementInput(InputAction.CallbackContext context)
    {
        //moveAxis = context.ReadValue<Vector2>();
        //Debug.Log(context.ReadValue<Vector2>());
    }

}
