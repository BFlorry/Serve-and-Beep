using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreventDeselectionGroup : MonoBehaviour
{
    private EventSystem evt;
    private GameObject selected = null;

    public static PreventDeselectionGroup Instance { get; private set; }

    private void Awake()
    {
        // If an older script of this type exists, destroy that.
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
        //evt = this.GetComponent<EventSystem>();
    }

    private void OnEnable()
    {
        evt = EventSystem.current;
        evt.SetSelectedGameObject(null);
    }

    private void Update()
    {
        evt = EventSystem.current;
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != selected)
            selected = evt.currentSelectedGameObject;
        else if (selected != null && selected.activeInHierarchy && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(selected);
    }
}