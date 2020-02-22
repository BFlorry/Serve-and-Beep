using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    bool carrying;
    GameObject carriedObject;
    GameObject carryPosition;

    [SerializeField]
    private float throwMagnitude = 100f;
    [SerializeField]
    private float carryOffset = 1f;

    [SerializeField]
    private float maxRayDistance = 0.5f;

    // Use this for initialization
    private void Start()
    {
        carryPosition = this.gameObject;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (carrying)
        {
            Carry(carriedObject);
        }
    }

    private void OnPickup()
    {
        if (carrying)
        {
            DropObject();
            return;
        }
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxRayDistance);
        Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.0f);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag == "Pickupable") {
                GameObject p = hit.collider.gameObject;
                if (p != null)
                    {
                        carrying = true;
                        carriedObject = p.gameObject;
                    }
            }
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
        if (carrying)
        {
            carrying = !carrying;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwMagnitude + new Vector3(0f, 200f, 0f) + GetComponent<Rigidbody>().velocity);
        }
    }

    private void DropObject()
    {
            carrying = !carrying;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity);
    }
}
