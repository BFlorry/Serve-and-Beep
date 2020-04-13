using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns item in player's hands.
/// </summary>
public class ItemSpawner : MonoBehaviour, IInteractable
{
    [SerializeField]
    private bool useDefaultPickupSound = false;

    [Tooltip("If this is left empty and Use Default Pickup Sound is false, no sound will be played.")]
    [SerializeField]
    private AudioClip spawnSound = null;

    [SerializeField]
    private GameObject itemPrefab;


    /// <summary>
    /// If interacter has PickupController, instantiates prefab and intercter pickups it.
    /// </summary>
    /// <param name="interacter">interacting GameObject</param>
    public void Interact(GameObject interacter)
    {
        if (interacter.TryGetComponent(out PickupController pCon))
        {
            GameObject spawnedItem = Instantiate(itemPrefab);
            if (spawnedItem.TryGetComponent(out Pickupable itemPickupable))
            {
                if (useDefaultPickupSound == true)
                {
                    pCon.Pickup(itemPickupable);
                }
                else
                {
                    pCon.Pickup(itemPickupable, spawnSound);
                }
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
