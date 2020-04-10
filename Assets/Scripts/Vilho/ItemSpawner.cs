using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject itemPrefab;

    public void Interact(GameObject interacter)
    {
        PickupController pCon = interacter.GetComponent<PickupController>();
        if (pCon != null)
        {
            Pickupable itemPickupable = Instantiate(itemPrefab).GetComponent<Pickupable>();
            if (itemPickupable != null)
            {
                pCon.Pickup(itemPickupable);
                Debug.Log(interacter.name + " picked up item " + itemPrefab.name);
            }
            else
            {
                Debug.LogWarning("Item Prefab " + itemPrefab.name + " has no Pickupable component. Can't be pickuped.");
            }
        }
        else
        {
            Debug.LogWarning("Interacter " + interacter.name + " has no PickupController component. Can't pic up item " + itemPrefab.name + ".");
        }
    }
}
