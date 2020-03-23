using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : TimedInteractionController
{
    PlayerManager playerManager;
    private void Start()
    {
        playerManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
    }
    public override void InteractionAction()
    {
        playerManager.LoadScene();
    }
}
