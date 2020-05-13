using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public GameObject CarriedObject { get; private set; }
    private GameObject carryPosition;

    [SerializeField]
    private AudioClip throwSfx;
    [SerializeField]
    private AudioClip pickupSfx;

    private PlayerSfxManager sfxManager;
    private HighlightCaster highlightCaster;
    [SerializeField]
    private float throwMagnitude = 100f;
    [SerializeField]
    private float carryOffsetFwd = 1f;
    [SerializeField]
    private float carryOffsetUp = 0.5f;

    [SerializeField]
    private float maxRayDistance = 0.5f;
    [SerializeField]
    private float maxRaySphereRadius = 0.5f;

    [SerializeField]
    private LayerMask carryCollideLayerMask;

    public bool Carrying { get; private set; }

    // Use this for initialization
    private void Start()
    {
        carryPosition = this.gameObject;
        sfxManager = GetComponent<PlayerSfxManager>();
        highlightCaster = this.GetComponent<HighlightCaster>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Carrying)
        {
            Carry(CarriedObject);
        }
    }

    public void TryPickup()
    {
        if (Carrying)
        {
            DropObject();
            return;
        }

        if (highlightCaster.TargetObject != null && highlightCaster.TargetObject.TryGetComponent<Pickupable>(out Pickupable pickupable))
        {
            Pickup(pickupable);
            return;
        }
    }

    public void Pickup(Pickupable pickupable)
    {
        sfxManager.PlaySingle(pickupSfx);
        PickupAction(pickupable);
    }

    public void Pickup(Pickupable pickupable, AudioClip sfx)
    {
        if (sfx != null)
        {
            sfxManager.PlaySingle(sfx);
        }
        PickupAction(pickupable);
    }

    public void PickupAction(Pickupable pickupable)
    {
        GameObject p = pickupable.gameObject;
        Carrying = true;
        CarriedObject = p.gameObject;
        pickupable.Pickup(this);
        CarriedObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Carry(GameObject o)
    {
        o.GetComponent<Rigidbody>().useGravity = false;
        MovePickupable(o);
        o.transform.rotation = carryPosition.transform.rotation;
    }

    /// <summary>
    /// Try to throw the pickupable the player is holding
    /// </summary>
    /// <returns>Did the throw succeed</returns>
    public bool Throw()
    {
        if (Carrying)
        {
            CarriedObject.GetComponent<Pickupable>().RemoveFromPlayer();
            sfxManager.PlaySingle(throwSfx);
            Carrying = false;
            CarriedObject.GetComponent<Rigidbody>().useGravity = true;
            CarriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwMagnitude + new Vector3(0f, 200f, 0f) + GetComponent<Rigidbody>().velocity);
            CarriedObject = null;

            return true;
        }
        else return false;
    }

    public void DropObject()
    {
        if (Carrying)
        {
            CarriedObject.GetComponent<Pickupable>().RemoveFromPlayer();
            Carrying = false;
            CarriedObject.GetComponent<Rigidbody>().useGravity = true;
            CarriedObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity);
            CarriedObject = null;
        }
    }

    public Pickupable GetPickupable()
    {
        if (CarriedObject == null) return null;
        if (CarriedObject.TryGetComponent<Pickupable>(out Pickupable pickupable))
        {
            return pickupable;
        }
        else return null;
    }

    private void MovePickupable(GameObject o)
    {
        Vector3 carryVector = carryPosition.transform.forward * carryOffsetFwd + carryPosition.transform.up * carryOffsetUp;

        RaycastHit hit;
        if (Physics.Raycast(carryPosition.transform.position, carryVector.normalized, out hit, carryVector.magnitude, carryCollideLayerMask))
        {
            // If hit something, keep object closer
            o.GetComponent<Rigidbody>().MovePosition(carryPosition.transform.position + carryVector.normalized * hit.distance);
        }
        else
        {
            // Didn't hit anything
            o.GetComponent<Rigidbody>().MovePosition(carryPosition.transform.position + carryVector);
        }
    }
}
