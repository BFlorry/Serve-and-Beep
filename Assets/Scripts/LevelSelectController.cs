using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    private string[] scenes;

    [SerializeField]
    private Button buttonLevel1;
    [SerializeField]
    private Button buttonLevel2;
    [SerializeField]
    private Button buttonLevel3;
    [SerializeField]
    private Button buttonLevel4;

    [SerializeField]
    StarDisplayController[] levelStars;

    [SerializeField]
    private Button buttonMenu;

    private GameStateController gameStateController;

    // Start is called before the first frame update
    void Start()
    {
        gameStateController = FindObjectOfType<GameStateController>();
        buttonLevel1.onClick.AddListener(() => LoadLevel(scenes[0]));
        buttonLevel2.onClick.AddListener(() => LoadLevel(scenes[1]));
        buttonLevel3.onClick.AddListener(() => LoadLevel(scenes[2]));
        buttonLevel4.onClick.AddListener(() => LoadLevel(scenes[3]));

        LoadLevelStars();

        buttonMenu.onClick.AddListener(() => gameStateController.LoadMenu());
    }

    private void LoadLevelStars()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            if(PlayerPrefs.HasKey(scenes[i] + "_stars"))
            {
                int stars = PlayerPrefs.GetInt(scenes[i] + "_stars");
                levelStars[i].EnableStar(stars);
            }
        }
    }

    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="sceneName">Desired scene name</param>
    private void LoadLevel(string sceneName)
    {
        FindObjectOfType<GameStateController>().SceneToLoad = sceneName;
        FindObjectOfType<GameStateController>().LoadLobby();
    }
}
