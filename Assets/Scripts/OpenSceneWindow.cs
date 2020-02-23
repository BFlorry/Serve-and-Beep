using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneWindow : MonoBehaviour, IInteractable {

    [SerializeField]
    string sceneName = "TestWindow";

    bool isInteracting = false;

    PlayerController playerController;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact(GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
        playerController.TogglePlayerFreeze();

        if (!isInteracting) {
            isInteracting = true;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            DeInteract();
        }
    }

    private void DeInteract()
    {
        isInteracting = false;
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
