using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private GameObject carriedObject;
    private GameObject carryPosition;

    [SerializeField]
    private AudioClip throwSfx;
    [SerializeField]
    private AudioClip pickupSfx;

    private PlayerSfxManager sfxManager;

    [SerializeField]
    private float throwMagnitude = 100f;
    [SerializeField]
    private float carryOffset = 1f;

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
            Carry(carriedObject);
        }
    }

    private void OnPickup()
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
        GameObject p = pickupable.gameObject;
        if (p != null)
        {
            sfxManager.PlaySingle(pickupSfx);
            Carrying = true;
            carriedObject = p.gameObject;
            pickupable.Pickup(this);
            return;
        }
    }


    private void Carry(GameObject o)
    {
        o.GetComponent<Rigidbody>().isKinematic = true;
        o.transform.position = carryPosition.transform.position + carryPosition.transform.forward * carryOffset;
        o.transform.rotation = carryPosition.transform.rotation;
    }

    private void OnThrow()
    {
        if (Carrying)
        {
            sfxManager.PlaySingle(throwSfx);
            Carrying = false;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwMagnitude + new Vector3(0f, 200f, 0f) + GetComponent<Rigidbody>().velocity);
            carriedObject.GetComponent<Pickupable>().Pickup(this);
            carriedObject = null;
        }
    }

    public void DropObject()
    {
        if (Carrying)
        {
            Carrying = false;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity);
            carriedObject = null;
        }
    }

    public Pickupable GetPickupable()
    {
        if (carriedObject == null) return null;
        if (carriedObject.TryGetComponent<Pickupable>(out Pickupable pickupable))
        {
            return pickupable;
        }
        else return null;
    }
    
}
