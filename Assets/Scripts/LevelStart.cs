using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : TimedInteractionController
{
    public override void InteractionAction()
    {
        string sceneToLoad = FindObjectOfType<GameStateController>().SceneToLoad;
        FindObjectOfType<GameStateController>().LoadStage(sceneToLoad);
    }
}
