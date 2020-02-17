using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    bool carrying;
    GameObject carriedObject;
    GameObject carryPosition;
    public float throwMagnitude = 100f;
    public float carryOffset = 10f;

    // Use this for initialization
    void Start()
    {
        carryPosition = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (carrying)
        {
            Carry(carriedObject);
        }
    }

    public void Pickup()
    {
        if (carrying)
        {
            DropObject();
            return;
        }
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            //Pickupable p = hit.collider.GetComponent<Pickupable>();
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

    void Carry(GameObject o)
    {
        o.GetComponent<Rigidbody>().isKinematic = true;
        o.transform.position = carryPosition.transform.position + carryPosition.transform.forward * carryOffset;
        o.transform.rotation = carryPosition.transform.rotation;
    }

    public void ThrowObject()
    {
        if (carrying)
        {
            carrying = !carrying;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwMagnitude + new Vector3(0f, 100f, 0f) + GetComponent<Rigidbody>().velocity);
        }
    }

    private void DropObject()
    {
            carrying = !carrying;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity);
    }
}
