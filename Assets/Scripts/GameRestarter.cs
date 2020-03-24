using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    private void OnReset()
    {
        GameObject.FindObjectOfType<PlayerManager>().EnableHotJoin();
        SceneManager.LoadSceneAsync(1);
    }
}
