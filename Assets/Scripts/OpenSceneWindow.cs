using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneWindow : MonoBehaviour, IInteractable {

    [SerializeField]
    string sceneName;

    bool isInteracting = false;

    PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact()
    {
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
