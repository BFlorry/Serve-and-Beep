using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreventDeselectionGroup : MonoBehaviour
{
    EventSystem evt;
    PlayerInput playerInput;
    GameObject selected = null;

    private void Awake()
    {
        evt = this.GetComponent<EventSystem>();
    }

    private void OnEnable()
    {
        evt.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != selected)
            selected = evt.currentSelectedGameObject;
        else if (selected != null && selected.activeInHierarchy && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(selected);
    }
}