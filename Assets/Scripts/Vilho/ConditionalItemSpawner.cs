using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums.Pickupables;

public class ConditionalItemSpawner : MonoBehaviour, IItemInteractable
{
    [SerializeField]
    private GameObject itemPrefab;

    [Serializable]
    private class ItemPair
    {
        public ItemType interactItemType;
        public GameObject spawnItem;
    }


    [SerializeField]
    private ItemPair[] itemPairArray;

    public bool Interact(GameObject interacter)
    {
        if (interacter.TryGetComponent<Pickupable>(out var pickupable))
        {
            foreach (ItemPair itempair in itemPairArray)
            {
                if (pickupable.ItemType.Equals(itempair.interactItemType))
                {
                    SpawnItemInHands(interacter, itempair.spawnItem);
                    return true;
                }
            }
        }
        return false;
    }


    private void SpawnItemInHands(GameObject player, GameObject prefab)
    {
        GameObject spawnedItem = Instantiate(prefab);
        if (spawnedItem.TryGetComponent<Pickupable>(out var pickupable))
        player.GetComponent<PickupController>().ChangeObject(pickupable);
    }


    //    PickupController pCon = interacter.GetComponent<PickupController>();
    //    if (pCon != null)
    //    {
    //        Pickupable itemPickupable = Instantiate(itemPrefab).GetComponent<Pickupable>();
    //        if (itemPickupable != null)
    //        {
    //            pCon.Pickup(itemPickupable);
    //            Debug.Log(interacter.name + " picked up item " + itemPrefab.name);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Item Prefab " + itemPrefab.name + " has no Pickupable component. Can't be pickuped.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Interacter " + interacter.name + " has no PickupController component. Can't pick up item " + itemPrefab.name + ".");
    //    }
    //}
}