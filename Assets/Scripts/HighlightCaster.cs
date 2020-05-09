using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCaster : MonoBehaviour
{
    enum Actions
    {
        Interact,
        Pickup
    }

    [SerializeField]
    private LayerMask highLightWithoutCarryingLayer;
    [SerializeField]
    private LayerMask highLightWhenCarryingLayer;

    [SerializeField]
    private float maxRayDistance = 0.5f;
    [SerializeField]
    private float maxRaySphereRadius = 0.5f;

    private Highlightable prevHighlight = null;
    private bool carrying;

    PickupController pickupController;

    ButtonPromptController buttonPrompt;

    public GameObject TargetObject { get; private set; } = null;

    private void Awake()
    {
        TryGetComponent(out pickupController);
        TryGetComponent(out buttonPrompt);
    }

    void Update()
    {
        carrying = pickupController.Carrying;
        HighLightObjects();
    }

    private void ShowButtonPrompt(Actions action)
    {
        if (action == Actions.Interact)
        {
            buttonPrompt.ShowInteract();
        }
        else if (action == Actions.Pickup)
        {
            buttonPrompt.ShowPickup();
        }
    }

    private void HighLightObjects()
    {
        bool clearPreviousHighlight = true;
        LayerMask curLayerMask;
        Highlightable curHighlight;

        if (carrying) curLayerMask = highLightWhenCarryingLayer;
        else curLayerMask = highLightWithoutCarryingLayer;

        Debug.DrawRay(this.transform.position, this.transform.forward * maxRayDistance, Color.magenta);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward, maxRaySphereRadius, transform.forward, maxRayDistance, curLayerMask);
        foreach (RaycastHit hit in hits)
        {
            MonoBehaviour[] targetList = hit.transform.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour mb in targetList)
            {
                if (carrying)
                {
                    if (mb is IItemInteractable)
                    {
                        if (hit.transform.gameObject.TryGetComponent(out curHighlight))
                        {
                            ShowButtonPrompt(Actions.Interact);
                            clearPreviousHighlight = ApplyHighlight(curHighlight, clearPreviousHighlight);
                            TargetObject = hit.transform.gameObject;
                            break;
                        }
                    }
                }
                else if (mb is Pickupable)
                {
                    if (hit.transform.gameObject.TryGetComponent(out curHighlight))
                    {
                        ShowButtonPrompt(Actions.Pickup);
                        clearPreviousHighlight = ApplyHighlight(curHighlight, clearPreviousHighlight);
                        TargetObject = hit.transform.gameObject;
                        break;
                    }
                }
                else if (mb is IInteractable)
                {
                    if (hit.transform.gameObject.TryGetComponent(out curHighlight))
                    {
                        if (mb is ItemSpawner)
                        {
                            ShowButtonPrompt(Actions.Pickup);
                        }
                        else ShowButtonPrompt(Actions.Interact);
                        clearPreviousHighlight = ApplyHighlight(curHighlight, clearPreviousHighlight);
                        TargetObject = hit.transform.gameObject;
                        break;
                    }
                }
                else if (hit.transform.gameObject.TryGetComponent(out curHighlight))
                {
                    // This case seems to happen with pickups
                    ShowButtonPrompt(Actions.Pickup);
                    clearPreviousHighlight = ApplyHighlight(curHighlight, clearPreviousHighlight);
                    TargetObject = hit.transform.gameObject;
                    break;
                }
            }
        }

        // If didn't hit any highlightable objects, clear previous highlight.
        if (clearPreviousHighlight && prevHighlight != null)
        {
            buttonPrompt.ClearPrompt();
            prevHighlight.UnHighlight();
            prevHighlight = null;
            TargetObject = null;
        }
    }

    private bool ApplyHighlight (Highlightable curHighlight, bool clearPreviousHighlight)
    {
        // If the new object isn't the same as previous, remove highlight from previous and apply to new
        if (curHighlight != prevHighlight)
        {
            if (prevHighlight != null) prevHighlight.UnHighlight();
            curHighlight.Highlight();
            prevHighlight = curHighlight;
        }
        clearPreviousHighlight = false;

        return clearPreviousHighlight;
    }

    public MonoBehaviour[] GetTargetObjects()
    {
        if (TargetObject != null)
        {
            return TargetObject.GetComponents<MonoBehaviour>();
        }
        return new MonoBehaviour[0];
    }
}
