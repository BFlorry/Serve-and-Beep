using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSnap : MonoBehaviour
{
    private Vector3 point;

    private void Start()
    {
        GameObject pointObj = transform.GetChild(0).gameObject;
        if (pointObj != null)
        {
            point = pointObj.transform.position;
        }
        else
        {
            point = this.transform.position;
            Debug.LogWarning("Snap point not found.");
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        Transform parent = collider.transform.parent;
        if (parent != null)
        {
            GameObject item = parent.gameObject;

            Pickupable pickupable = item.GetComponent<Pickupable>();

            if (pickupable != null)
            {
                if (pickupable.Carried == false)
                {
                    item.transform.position = point;
                    parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}
