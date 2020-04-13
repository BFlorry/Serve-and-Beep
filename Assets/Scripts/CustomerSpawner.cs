using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject customer;

    [SerializeField]
    int maxAmountOfCustomers = 5;

    [SerializeField]
    float customerSpawnInterval = 10f;

    List<GameObject> customers;

    Vector3 spawnPosition;
    Quaternion spawnRotation;


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        DrawArrow.ForGizmo(this.transform.position - this.transform.forward, this.transform.forward * 2f, Color.white, 0.25f, 20f, 1f);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = this.transform.position + Vector3.back;
        spawnRotation = this.transform.rotation;
        customers = new List<GameObject>();
        StartCoroutine(SpawnCustomer());
    }

    // Update is called once per frame
    void Update()
    {

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
            if (customers.Count < maxAmountOfCustomers)
            {
                Debug.Log("Spawning a new customer...");
                customers.Add(Instantiate(customer, spawnPosition, spawnRotation));
            }
            else
            {
                Debug.Log("Tried to spawn a customer, but there's too many.");
            }
            yield return new WaitForSeconds(customerSpawnInterval);
        }
    }
}
