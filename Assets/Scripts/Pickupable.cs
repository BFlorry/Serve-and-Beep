using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle pickup actions and interactions from player.
/// </summary>
public class Pickupable : MonoBehaviour
{
    [SerializeField]
    private float maxRayDistance = 2.0f;

    [SerializeField]
    AudioClip interactSfx;

    PickupController player;

    //TODO: Maybe implement item type this way?
    //[SerializeField]
    //Item type = Item.Crate;

    [SerializeField]
    GameObject[] targetInteractableObjects;

    List<IItemInteractable> targetInteractables;

    private void Start()
    {
        targetInteractables = new List<IItemInteractable>();
        foreach (GameObject interactableObject in targetInteractableObjects)
        {
            if(interactableObject.TryGetComponent(out IItemInteractable interactable)) targetInteractables.Add(interactable);
        }
    }

    public void Pickup (PickupController pickupPlayer)
    {
        if(player == null) player = pickupPlayer;
        else if (player.Equals(pickupPlayer) == false)
        {
            player.DropObject();
            player = pickupPlayer;
        }
    }

    /// <summary>
    /// Raycast forward and if there is an iteminteractable object, interact with it and (currently) destroy this object.
    /// </summary>
    public void InteractWithItem()
    {
        //TODO: This as a separate class?
        //TODO: Replace with spherecast
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxRayDistance);
        Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.red, 0.0f);
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
                                player.GetComponent<PlayerSfxManager>().PlaySingle(interactSfx);
                                player.DropObject();
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
