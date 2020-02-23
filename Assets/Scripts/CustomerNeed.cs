using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNeed : MonoBehaviour, IItemInteractable
{
    enum Need
    {
        Empty,
        Hungry
    }

    [SerializeField]
    Need curNeed;
    Customer customer;

    [SerializeField]
    float maxValue = 100f;
    [SerializeField]
    float currentValue;
    [SerializeField]
    float defaultValue = 0f;
    [SerializeField]
    int satisfactionModifier = 20;

    public float MaxValue { get => maxValue; }
    public float CurrentValue { get => currentValue; }
    public float DefaultValue { get => defaultValue; }

    // Start is called before the first frame update
    void Start()
    {
        currentValue = defaultValue;
        customer = GetComponent<Customer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(curNeed == Need.Hungry)
        {
            if (currentValue < maxValue) currentValue += 0.05f;
            else
            {
                curNeed = Need.Empty;
                currentValue = defaultValue;
                customer.SfGain(-satisfactionModifier);
            }
        }
    }
    
    public void Interact(GameObject gameObject)
    {
        if(curNeed == Need.Hungry)
        {
            curNeed = Need.Empty;
            currentValue = defaultValue;
            customer.SfGain(satisfactionModifier);
        }
    }
}
