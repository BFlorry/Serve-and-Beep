using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums.Pickupables;

/// <summary>
/// Handle pickup actions and interactions from player.
/// </summary>
public class Pickupable : MonoBehaviour
{
    //Fields---------------------------------------------------------------------------

    [SerializeField]
    private ItemType itemType;

    [SerializeField]
    private float maxRayDistance = 1.0f;
    [SerializeField]
    private float maxRaySphereRadius = 0.5f;
    [SerializeField]
    private GameObject[] targetInteractableObjects;


    //TODO: Maybe implement item type this way?
    //[SerializeField]
    //Item type = Item.Crate;


    private List<IItemInteractable> targetInteractables;


    //Properties-----------------------------------------------------------------------

    public PickupController Player { get; private set; }
    public ItemSnap ItemSnap {
        get;
        set; } = null;
    public bool Carried { get; set; } = false;

    public ItemType ItemType { get => itemType; }


    //Methods--------------------------------------------------------------------------

    private void Start()
    {
        targetInteractables = new List<IItemInteractable>();
        foreach (GameObject interactableObject in targetInteractableObjects)
        {
            if (interactableObject.TryGetComponent(out IItemInteractable interactable)) targetInteractables.Add(interactable);
        }
    }

    public void Pickup(PickupController pickupPlayer)
    {
        if (Player == null)
        {
            NullifyItemSnap();
            // No player carrying, set this player
            Carried = true;
            Player = pickupPlayer;
        }
        else if (Player.Equals(pickupPlayer) == false)
        {
            // Change player
            DropObjFromPlayer();
            Carried = true;
            Player = pickupPlayer;
        }
    }

    public void DropObjFromPlayer()
    {
        Carried = false;
        if (Player != null)
        {
            Player.DropObject();
            Player = null;
        }
    }

    public void RemoveFromPlayer()
    {
        Carried = false;
        Player = null;
    }

    /// <summary>
    /// Raycast forward and if there is an iteminteractable object, interact with it and (currently) destroy this object.
    /// </summary>
    public bool InteractWithItem()
    {
        //TODO: This as a separate class?
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, maxRaySphereRadius, transform.forward, maxRayDistance);
        foreach (RaycastHit hit in hits)
        {
            MonoBehaviour[] targetList = hit.transform.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour mb in targetList)
            {
                if (mb is IItemInteractable)
                {
                    IItemInteractable interactable = (IItemInteractable)mb;
                    foreach (IItemInteractable interactObj in targetInteractables)
                    {
                        bool interactSuccess = interactable.Interact(this.gameObject);
                        if (interactSuccess == true)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }
        return false;
    }

    public void DestroyPickupable(float time = 0f)
    {
        DropObjFromPlayer();
        StartCoroutine(DestroyAfterTime(this.gameObject, time));
    }

    /// <summary>
    /// Destroys item and its children after given time.
    /// </summary>
    /// <param name="obj">GameObject to be destroyed.</param>
    /// <param name="time">time to be waited</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.FindObjectOfType<PickupableManager>().DespawnPickupable(obj);
    }

    public void NullifyItemSnap()
    {
        if (ItemSnap != null)
        {
            ItemSnap.SnappedItem = null;
            ItemSnap = null;
        }
    }
}
