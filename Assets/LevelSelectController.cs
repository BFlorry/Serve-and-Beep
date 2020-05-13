using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    private Button buttonLevel1;
    [SerializeField]
    private Button buttonLevel2;
    [SerializeField]
    private Button buttonLevel3;
    [SerializeField]
    private Button buttonLevel4;

    [SerializeField]
    private Button buttonMenu;

    private GameStateController gameStateController;

    // Start is called before the first frame update
    void Start()
    {
        gameStateController = FindObjectOfType<GameStateController>();
        buttonLevel1.onClick.AddListener(() => LoadLevel(1));
        buttonLevel2.onClick.AddListener(() => LoadLevel(2));
        buttonLevel3.onClick.AddListener(() => LoadLevel(3));
        buttonLevel4.onClick.AddListener(() => LoadLevel(4));

        buttonMenu.onClick.AddListener(() => gameStateController.LoadMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="levelNumber">Desired level number, eg. Level 1</param>
    private void LoadLevel(int levelNumber)
    {
        FindObjectOfType<GameStateController>().LoadStage(levelNumber - 1);
    }
}
