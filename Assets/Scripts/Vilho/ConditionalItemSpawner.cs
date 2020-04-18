using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums.Pickupables;

/// <summary>
/// Changes item in player's hands according to the interacter's itemType.
/// </summary>
public class ConditionalItemSpawner : MonoBehaviour, IItemInteractable
{
    //Fields--------------------------------------------------------------------------------------------

    [Tooltip("If checked, overrides Spawn Sound.")]
    [SerializeField]
    private bool useDefaultPickupSound = false;

    [Tooltip("If this is left empty and Use Default Pickup Sound is false, no sound will be played.")]
    [SerializeField]
    private AudioClip spawnSound = null;

    [Serializable]
    private class ItemPair
    {
        public ItemType interactItemType;
        public GameObject spawnItem;
    }

    [SerializeField]
    private ItemPair[] itemPairArray;


    //Methods--------------------------------------------------------------------------------------------

    /// <summary>
    /// Checks if there is same ItemType in itemPairArray as the interacter's
    /// Pickupable component's Itemtype. If yes calls for spawner accordingly.
    /// </summary>
    /// <param name="interacter">GameObject that interacts.</param>
    /// <returns>True if interact was succesful, else false.</returns>
    public bool Interact(GameObject interacter)
    {
        if (interacter.TryGetComponent(out Pickupable pickupable))
        {
            foreach (ItemPair itempair in itemPairArray)
            {
                if (pickupable.ItemType.Equals(itempair.interactItemType))
                {
                    if (pickupable.Player.TryGetComponent(out PickupController pickupCtrl))
                    {
                        SpawnItemInHands(pickupCtrl, itempair.spawnItem, interacter.transform);

                    }
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Spawns item in hands.
    /// </summary>
    /// <param name="pickupCtrl">PickupController to pickup spawned item.</param>
    /// <param name="prefab">Prefab that is instantiated.</param>
    /// <param name="spawnTransform">Spawn transform.</param>
    private void SpawnItemInHands(PickupController pickupCtrl, GameObject prefab, Transform spawnTransform)
    {
        if (pickupCtrl.CarriedObject.TryGetComponent(out Pickupable curPickupable))
        {
            pickupCtrl.DropObject();
            curPickupable.DestroyPickupable();
        }

        GameObject spawnedItem = FindObjectOfType<PickupableManager>().SpawnPickupable(prefab, spawnTransform.position, spawnTransform.rotation);

        if (spawnedItem.TryGetComponent<Pickupable>(out Pickupable pickupable))
        {
            if (useDefaultPickupSound == true)
            {
                pickupCtrl.Pickup(pickupable);
            }
            else
            {
                pickupCtrl.Pickup(pickupable, spawnSound);
            }
        }
    }
}