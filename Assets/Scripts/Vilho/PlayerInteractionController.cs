using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField]
    private float maxRayDistance = 0.5f;
    [SerializeField]
    private float maxRaySphereRadius = 0.5f;

    private PickupController pickupController;
    private HighlightCaster highlightCaster;


    private void Start()
    {
        pickupController = this.GetComponent<PickupController>();
        highlightCaster = this.GetComponent<HighlightCaster>();
    }

    /// <summary>
    /// Creates SphereCast from this GameObject, and a list of objects hit, that implement IInteractable.
    /// Calls every list member's Interact function.
    /// </summary>
    /// <returns>Returns whether interact happened</returns>
    public bool Interact()
    {
        Pickupable pickupable = pickupController.GetPickupable();
        if (pickupable != null)
        {
            Debug.Log("Sending interaction call to picked up item...");
            return pickupable.InteractWithItem();
        }
        else
        {
            MonoBehaviour[] targetList = highlightCaster.GetTargetObjects();

            foreach (MonoBehaviour mb in targetList)
            {
                if (mb is IInteractable)
                {
                    SendInteract(mb, this.gameObject);
                    Debug.Log("Sending interaction call to interactable item...");
                    return true;
                }
            }
        }
        return false;
    }

    private void SendInteract(MonoBehaviour mb, GameObject gameobject)
    {
        IInteractable interactable = (IInteractable)mb;
        interactable.Interact(gameobject);
        Debug.Log("Sending interaction call to interactable target...");
    }

}