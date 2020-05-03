using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCaster : MonoBehaviour
{
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

    public GameObject TargetObject { get; private set; } = null;

    private void Awake()
    {
        TryGetComponent(out pickupController);
    }

    void Update()
    {
        carrying = pickupController.Carrying;
        HighLightObjects();
    }

    private void HighLightObjects()
    {
        bool clearPreviousHighlight = true;
        LayerMask curLayerMask;
        if (carrying) curLayerMask = highLightWhenCarryingLayer;
        else curLayerMask = highLightWithoutCarryingLayer;

        Debug.DrawRay(this.transform.position, this.transform.forward * maxRayDistance, Color.magenta);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward, maxRaySphereRadius, transform.forward, maxRayDistance, curLayerMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.TryGetComponent(out Highlightable curHighlight))
            {
                // If the new object isn't the same as previous, remove highlight from previous and apply to new
                if (curHighlight != prevHighlight)
                {
                    if (prevHighlight != null) prevHighlight.UnHighlight();
                    curHighlight.Highlight();
                    prevHighlight = curHighlight;
                }
                clearPreviousHighlight = false;
                TargetObject = hit.transform.gameObject;
                break;
            }
        }

        // If didn't hit any highlightable objects, clear previous highlight.
        if (clearPreviousHighlight && prevHighlight != null)
        {
            prevHighlight.UnHighlight();
            prevHighlight = null;
            TargetObject = null;
        }
    }
}
