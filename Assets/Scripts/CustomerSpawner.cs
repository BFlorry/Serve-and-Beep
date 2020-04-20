using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject customer;

    [SerializeField]
    private int customersPerPlayer = 5;

    private int totalCustomerAmount = 0;

    [SerializeField]
    private float customerSpawnInterval = 10f;

    List<GameObject> customers = new List<GameObject>();

    Vector3 spawnPosition;
    Quaternion spawnRotation;

    public AudioClip enterSfx;

    public AudioSource Audio;


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        DrawArrow.ForGizmo(this.transform.position - this.transform.forward, this.transform.forward * 2f, Color.white, 0.25f, 20f, 1f);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerInput.all.Count > 0)
        {
            totalCustomerAmount = customersPerPlayer * PlayerInput.all.Count;
        }
        else
        {
            totalCustomerAmount = customersPerPlayer;
            StartCoroutine(LateCheckPlayerAmount());
        }
        spawnPosition = this.transform.position + Vector3.back;
        spawnRotation = this.transform.rotation;
        StartCoroutine(SpawnCustomer());
    }

    /// <summary>
    /// Players might not be loaded to scene before after Start(), so wait for 0.5 seconds and check again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LateCheckPlayerAmount()
    {
        yield return new WaitForSeconds(0.5f);
        if (PlayerInput.all.Count > 0)
        {
            totalCustomerAmount = customersPerPlayer * PlayerInput.all.Count;
        }
    }

    public void DespawnCustomer(GameObject customer)
    {
        customers.Remove(customer);
        Destroy(customer);
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            if (customers.Count < totalCustomerAmount)
            {
                Debug.Log("Spawning a new customer...");
                customers.Add(Instantiate(customer, spawnPosition, spawnRotation));
                Audio.PlayOneShot(enterSfx);

            }
            else
            {
                Debug.Log("Tried to spawn a customer, but there's too many.");
            }
            yield return new WaitForSeconds(customerSpawnInterval);
        }
    }
}
