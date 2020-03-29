using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : TimedInteractionController
{
    public override void InteractionAction()
    {
        FindObjectOfType<GameStateController>().LoadStage(("Main_Ship"));
    }
}
