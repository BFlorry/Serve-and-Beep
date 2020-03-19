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

    private IEnumerator waitAfterMove;

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

    [SerializeField]
    float timeBetweenNeeds = 3f;

    public float MaxValue { get => maxValue; }
    public float CurrentValue { get => currentValue; }
    public float DefaultValue { get => defaultValue; }

    // Start is called before the first frame update
    void Start()
    {
        currentValue = defaultValue;
        customer = GetComponent<Customer>();
    }

    void Update()
    {
        if(curNeed == Need.Empty) StartCoroutine(WaitBeforeNext(timeBetweenNeeds));
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
                customer.SfGain(-satisfactionModifier);
            }
        }
    }
    /// <summary>
    /// Accepts any interactable item and excecutes the customer's need completion.
    /// </summary>
    /// <param name="gameObject"></param>
    public void Interact(GameObject gameObject)
    {
        if(curNeed == Need.Hungry)
        {
            curNeed = Need.Empty;
            customer.SfGain(satisfactionModifier);
        }
    }

    private IEnumerator WaitBeforeNext(float time)
    {
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        curNeed = Need.Hungry;
    }
}
