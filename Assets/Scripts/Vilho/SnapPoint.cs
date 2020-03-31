using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour, IInteractable
{
    //public void Interact(GameObject gameObj)
    //{
    //    PickupController pickupCtrl = gameObj.GetComponent<PickupController>();
    //    if (pickupCtrl != null)
    //    {
    //        GameObject item = pickupCtrl.CarriedObject;
    //        if (item != null)
    //        {
    //            pickupCtrl.DropObject();
    //            item.transform.position = this.transform.position;
    //            Debug.Log("GameObject " + item.name + " placed on snap point.");
    //        }
    //    }
    //}

    public void Interact(GameObject gameObject)
    {
        Pickupable pickupable = gameObject.GetComponent<Pickupable>();

        if (pickupable != null)
        {
            pickupable.Player.DropObject();
            gameObject.transform.position = this.transform.position;
            Debug.Log("GameObject " + gameObject.name + " placed on snap point.");
        }
        else
        {
            Debug.Log("GameObject " + gameObject.name +
                " interacting with SnapPoint is not Pickupable. It gets ignored.");
        }
    }
}
