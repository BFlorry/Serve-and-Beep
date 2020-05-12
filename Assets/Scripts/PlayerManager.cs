using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayerManager : MonoBehaviour
{
    PlayerData[] m_Players;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private AudioClip joinSfx;

    public string myFirstScene;

    private SoundManager soundManager;

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
        // Return the current Active Scene in order to get the current Scene name.
        Scene scene = SceneManager.GetActiveScene();

        // Check if the name of the current Active Scene is your first Scene.
        if (scene.name == myFirstScene)
        {
            soundManager = FindObjectOfType<SoundManager>();
            soundManager.PlaySingle(joinSfx);

        }


        foreach (PlayerInput playerInput in PlayerInput.all)
        {
            if( playerInput != null)
            {
                playerInput.TryGetComponent(out PlayerMaterialManager playerMaterialManager);
                if (playerMaterialManager != null)
                {
                    playerMaterialManager.SetPlayerIndex(playerInput.playerIndex);
                }
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
        GameObject[] spawnPoints = FindObjectOfType<LevelManager>().PlayerSpawnpoints;
        foreach (PlayerData player in m_Players)
        {
            if(player != null)
            {
                PlayerInput input = PlayerInput.Instantiate(playerPrefab, /*controlScheme: nameOfControlSchemeToUse,*/ playerIndex: player.index, pairWithDevices: player.devices);
                input.transform.position = spawnPoints[player.index].transform.position;

            }
        }

        // TODO: If player customization by material is going to be enabled, edit this out and make the color persist some other way
        foreach (PlayerInput playerInput in PlayerInput.all)
        {
            if(playerInput != null)
            {
                playerInput.TryGetComponent(out PlayerMaterialManager playerMaterialManager);
                if (playerMaterialManager != null)
                {
                    playerMaterialManager.SetPlayerIndex(playerInput.playerIndex);
                }
            }
        }
    }

    public void DeserializePlayers()
    {
        foreach (PlayerInput player in PlayerInput.all)
        {
            if(player != null)
            {
                player.user.UnpairDevicesAndRemoveUser();
                player.DeactivateInput();
                Destroy(player);
            }
        }
        DisableHotJoin();
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    public void LoadScene(int scene)
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

    IEnumerator LoadSceneAsync(int scene)
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
