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
    GameObject sel;

    private void Awake()
    {
        evt = this.GetComponent<EventSystem>();
        //playerInput = this.GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        evt.SetSelectedGameObject(null);
        /*if(playerInput.enabled)
        {
            ForceCheckCamera();
        }*/
    }

    public void ForceCheckCamera()
    {
        playerInput.camera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        if (playerInput.camera == null) playerInput.camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {/*
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != sel)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
        else
        {
            Button button = FindObjectOfType<Button>();
            if(button != null)
            {
                sel = button.gameObject;
                evt.SetSelectedGameObject(sel);
            }
        }*/
        if ( evt.currentSelectedGameObject == null || evt.currentSelectedGameObject.activeInHierarchy == false)
        {
            evt.SetSelectedGameObject(null);
            Button button = FindObjectOfType<Button>();
            if (button != null)
            {
                sel = button.gameObject;
                evt.SetSelectedGameObject(sel);
            }
        }
    }
}