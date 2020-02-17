using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    //[SerializeField]
    //private KeyCode interactionKey = KeyCode.I;

    [SerializeField]
    private float
        maxRayDistance = 2.0f;


    private void FixedUpdate()
    {
        /*if (Input.GetKey(interactionKey))
        {
            TryInteraction();
        }*/
    }


    /// <summary>
    /// Creates RayCast from this GameObject, and a list of objects hit, that implement IInteractable.
    /// Calls every list member's Interact function.
    /// </summary>
    public void TryInteraction()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.red, 0.0f);

        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            MonoBehaviour[] targetList = hit.transform.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour mb in targetList)
            {
                if (mb is IInteractable)
                {
                    IInteractable interactable = (IInteractable)mb;
                    interactable.Interact();
                    Debug.Log("Sending interaction call to interactable target.");
                }
            }
        }
    }


}