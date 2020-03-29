using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageOverManager : MonoBehaviour
{
    [SerializeField]
    Button restartButton;

    [SerializeField]
    Button menuButton;

    [SerializeField]
    Button quitButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameStateController gameStateController = FindObjectOfType<GameStateController>();
        restartButton.onClick.AddListener(() => gameStateController.RestartScene());
        menuButton.onClick.AddListener(() => gameStateController.LoadMenu());
        quitButton.onClick.AddListener(() => gameStateController.QuitGame());
    }
}
