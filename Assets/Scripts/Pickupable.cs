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
    private float maxRayDistance = 2.0f;
    [SerializeField]
    private float maxRaySphereRadius = 0.5f;
    [SerializeField]
    private AudioClip interactSfx;
    [SerializeField]
    private GameObject[] targetInteractableObjects;


    //TODO: Maybe implement item type this way?
    //[SerializeField]
    //Item type = Item.Crate;


    private List<IItemInteractable> targetInteractables;


    //Properties-----------------------------------------------------------------------

    public PickupController Player { get; private set; }
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
            Player = pickupPlayer;
        }
        else if (Player.Equals(pickupPlayer) == false)
        {
            RemoveFromPlayer();
            Player = pickupPlayer;
        }
    }

    public void RemoveFromPlayer()
    {
        if (Player != null)
        {
            Player.DropObject(); Player = null;
        }

    }

    /// <summary>
    /// Raycast forward and if there is an iteminteractable object, interact with it and (currently) destroy this object.
    /// </summary>
    public void InteractWithItem()
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
                        if (interactObj.GetType() == interactable.GetType())
                        {
                            bool interactSuccess = interactable.Interact(this.gameObject);
                            if (interactSuccess == true)
                            {
                                Player.GetComponent<PlayerSfxManager>().PlaySingle(interactSfx);
                                RemoveFromPlayer();
                                Destroy(this.gameObject);
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
}
