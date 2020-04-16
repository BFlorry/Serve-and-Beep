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

    public bool Carrying { get; private set; }

    // Use this for initialization
    private void Start()
    {
        carryPosition = this.gameObject;
        sfxManager = GetComponent<PlayerSfxManager>();
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

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, maxRaySphereRadius, transform.forward, maxRayDistance);
        Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.0f);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.TryGetComponent<Pickupable>(out Pickupable pickupable))
            {
                Pickup(pickupable);
            }
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
        CarriedObject.GetComponent<Pickupable>().Carried = true;
        pickupable.Pickup(this);
    }

    private void Carry(GameObject o)
    {
        o.GetComponent<Rigidbody>().isKinematic = true;
        o.transform.position = carryPosition.transform.position + carryPosition.transform.forward * carryOffsetFwd + carryPosition.transform.up * carryOffsetUp;
        o.transform.rotation = carryPosition.transform.rotation;
    }

    private void OnThrow()
    {
        if (Carrying)
        {
            CarriedObject.GetComponent<Pickupable>().Carried = false;
            sfxManager.PlaySingle(throwSfx);
            Carrying = false;
            CarriedObject.GetComponent<Rigidbody>().isKinematic = false;
            CarriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwMagnitude + new Vector3(0f, 200f, 0f) + GetComponent<Rigidbody>().velocity);
            CarriedObject.GetComponent<Pickupable>().Pickup(this);
            CarriedObject = null;
        }
    }

    public void DropObject()
    {
        if (Carrying)
        {
            CarriedObject.GetComponent<Pickupable>().Carried = false;
            Carrying = false;
            CarriedObject.GetComponent<Rigidbody>().isKinematic = false;
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
}
