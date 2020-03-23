using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    PlayerData[] m_Players;

    [SerializeField]
    private GameObject playerPrefab;

    private void DisableHotPlay()
    {
        gameObject.GetComponent<PlayerInputManager>().DisableJoining();
    }

    /// <summary>
    /// Store players for scene change
    /// </summary>
    public void StorePlayers()
    {
        //Disable hot joining when storing players.
        DisableHotPlay();

        m_Players = PlayerInput.all.Select(x => new PlayerData
        {
            devices = x.devices.ToArray(),
            index = x.playerIndex
        }).ToArray();
    }

    /// <summary>
    /// Instantiate players after scene change
    /// </summary>
    public void LoadPlayers()
    {
        foreach (var player in m_Players)
            PlayerInput.Instantiate(playerPrefab, /*controlScheme: nameOfControlSchemeToUse,*/ playerIndex: player.index, pairWithDevices: player.devices);
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        StorePlayers();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main_Ship");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        LoadPlayers();
    }


    private class PlayerData
    {
        public InputDevice[] devices { get; set; }
        public int index { get; set; }
    }
}
