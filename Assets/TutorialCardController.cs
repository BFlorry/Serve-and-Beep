using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialCardController : MonoBehaviour
{

    [SerializeField]
    private GameObject tutorialCard;
    private void OnEnable()
    {
        tutorialCard.SetActive(true);
    }

    private void Update()
    {
        if(Time.timeScale > 0)
            Time.timeScale = 0f;
        if (Keyboard.current.kKey.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            Destroy(this.gameObject);
        }
    }
}
