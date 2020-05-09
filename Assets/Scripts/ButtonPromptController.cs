using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class ButtonPromptController : MonoBehaviour
{
    [SerializeField]
    Image buttonPrompt;

    [SerializeField]
    private GameObject buttonPromptCanvas;

    [SerializeField]
    Sprite keyboardInteract;
    [SerializeField]
    Sprite keyboardPickup;

    [SerializeField]
    Sprite dualshockInteract;
    [SerializeField]
    Sprite dualshockPickup;

    [SerializeField]
    Sprite xboxInteract;
    [SerializeField]
    Sprite xboxPickup;

    private void OnEnable()
    {
        Debug.Log(GetComponent<PlayerInput>().devices.First().displayName);
        Debug.Log(GetComponent<PlayerInput>().devices.First().layout);
        ClearPrompt();
    }

    private void Update()
    {
        if(buttonPrompt.sprite != null && buttonPrompt.enabled)
        {
            Camera camera = FindObjectOfType<Camera>();
            buttonPromptCanvas.transform.LookAt(buttonPromptCanvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }
    }

    public void ShowInteract()
    {
        Enable();
        if (GetComponent<PlayerInput>().devices.First().layout.Contains("DualShock"))
        {
            buttonPrompt.sprite = dualshockInteract;
            return;
        }
        else if (GetComponent<PlayerInput>().devices.First().layout.Contains("XInput"))
        {
            buttonPrompt.sprite = xboxInteract;
            return;
        }
        else if (GetComponent<PlayerInput>().devices.First().layout.Contains("Keyboard"))
        {
            buttonPrompt.sprite = keyboardInteract;
            return;
        }

        // Fallback
        buttonPrompt.sprite = keyboardInteract;
    }

    public void ShowPickup()
    {
        Enable();
        if (GetComponent<PlayerInput>().devices.First().layout.Contains("DualShock"))
        {
            buttonPrompt.sprite = dualshockPickup;
            return;
        }
        else if (GetComponent<PlayerInput>().devices.First().layout.Contains("XInput"))
        {
            buttonPrompt.sprite = xboxPickup;
            return;
        }
        else if (GetComponent<PlayerInput>().devices.First().layout.Contains("Keyboard"))
        {
            buttonPrompt.sprite = keyboardPickup;
            return;
        }

        // Fallback
        buttonPrompt.sprite = keyboardPickup;
    }

    public void ClearPrompt()
    {
        Disable();
    }
    private void Enable()
    {
        buttonPrompt.enabled = true;
    }
    private void Disable()
    {
        buttonPrompt.enabled = false;
    }
}
