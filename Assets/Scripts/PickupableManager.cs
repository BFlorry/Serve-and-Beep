using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupableManager : MonoBehaviour
{
    [SerializeField]
    int maxAmountOfPickupables = 10;

    [SerializeField]
    float pickupableCheckInterval = 0.5f;

    List<GameObject> pickupables = new List<GameObject>();

    public void DespawnPickupable(GameObject pickup)
    {
        pickupables.Remove(pickup);
        Destroy(pickup);
    }

    public GameObject SpawnPickupable(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject spawnedInstance = Instantiate(prefab, position, rotation);
        RegisterPickupable(spawnedInstance);
        return spawnedInstance;
    }

    public void RegisterPickupable(GameObject newPickup)
    {
        pickupables.Add(newPickup);
        // If there's more than max amount of pickupables, despawn earliest one.
        if (pickupables.Count > maxAmountOfPickupables)
        {
            FindWhatCanBeDespawned();
        }
    }

    void FindWhatCanBeDespawned()
    {
        // Check up to until 5th pickupable if it's currently carried to avoid despawning while in carry.
        for (int i = 0; i <= 5; i++)
        {
            if (pickupables[i].TryGetComponent(out Pickupable pickup))
            {
                if (!pickup.Carried)
                {
                    DespawnPickupable(pickupables[i]);
                    Debug.Log("Despawned oldest object, because there's too many.");
                    return;
                }
            }
        }
        Debug.LogWarning("Too many pickupables, but couldn't despawn one!");
    }
}
