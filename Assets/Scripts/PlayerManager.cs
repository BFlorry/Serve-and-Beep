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

    private void DisableHotJoin()
    {
        gameObject.GetComponent<PlayerInputManager>().DisableJoining();
    }

    public void EnableHotJoin()
    {
        gameObject.GetComponent<PlayerInputManager>().EnableJoining();
    }

    private void OnPlayerJoined()
    {
        foreach (PlayerInput playerInput in PlayerInput.all)
        {
            playerInput.TryGetComponent(out PlayerMaterialManager playerMaterialManager);
            if (playerMaterialManager != null)
            {
                playerMaterialManager.SetPlayerIndex(playerInput.playerIndex);
            }
        }
    }

    /// <summary>
    /// Store players for scene change
    /// </summary>
    public void StorePlayers()
    {
        //Disable hot joining when storing players.
        DisableHotJoin();

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
        foreach (PlayerData player in m_Players)
            PlayerInput.Instantiate(playerPrefab, /*controlScheme: nameOfControlSchemeToUse,*/ playerIndex: player.index, pairWithDevices: player.devices);
    }

    public void DeserializePlayers()
    {
        /*foreach (var player in m_Players)
            PlayerInput.Destroy()*/
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        StorePlayers();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

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
        public Material playerMaterial { get; set; }
    }

}
