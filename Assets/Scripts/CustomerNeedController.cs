using System;
using System.Collections;
using UnityEngine;
using static Enums.CustomerEnums;

/// <summary>
/// A class that contains and controls a customer's needs and the needs' areas.
/// </summary>
public class CustomerNeedController : MonoBehaviour, IItemInteractable
{
    //Fields------------------------------------------------------------------

    [SerializeField]
    CustomerNeed curCustomerNeed = new CustomerNeed(NeedNameEnum.Empty);

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

    Customer customer;
    AIController aiController;
    CustomerAreas customerAreas;

    //The customer will be waiting for a new
    //need after finishing previous one.
    private bool isWaiting = false;

    private IEnumerator waitAfterMove;

    //Properties----------------------------------------------------------------

    public float MaxValue { get => maxValue; }
    public float CurrentValue { get => currentValue; }
    public float DefaultValue { get => defaultValue; }


    // Start is called before the first frame update
    void Start()
    {
        this.currentValue = defaultValue;
        this.customer = GetComponent<Customer>();
        this.aiController = GetComponent<AIController>();
        this.customerAreas = GetComponent <CustomerAreas>();
        SetNeed(GetRandomEnum<NeedNameEnum>());
    }


    void Update()
    {
        if(curCustomerNeed.NeedName == NeedNameEnum.Empty && isWaiting == false)
        {
            isWaiting = true;
            StartCoroutine(WaitBeforeNextNeed(timeBetweenNeeds));
        }
        
        //For testing only
        SetNeedWithKeyboard();
    }


    /// <summary>
    /// Sets this customer's current need as given need.
    /// Gets area that the given need belongs to.
    /// Sets ai controller's movement to that area.
    /// </summary>
    /// <param name="need">A need to be set to this customer.</param>
    public void SetNeed(NeedNameEnum needName)
    {
        curCustomerNeed = new CustomerNeed(needName);
        aiController.MoveToNeedDestination(curCustomerNeed);
    }


    /// <summary>
    /// Method for testing. Sets need as the number key that was pressed.
    /// </summary>
    private void SetNeedWithKeyboard()
    {
        int number;
        if (Int32.TryParse(Input.inputString, out number))
        {
            if (number >= 0 && number <= 9)
            {
                SetNeed((NeedNameEnum)number);
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(curCustomerNeed.NeedName == NeedNameEnum.Hunger)
        {
            if (currentValue < maxValue) currentValue += 0.05f;
            else
            {
                SetNeed(NeedNameEnum.Empty);
                customer.SfGain(-satisfactionModifier);
            }
        }
    }


    /// <summary>
    /// Accepts any interactable item and excecutes the customer's need completion.
    /// </summary>
    /// <param name="gameObject">interacting gameobject</param>
    /// <returns>true if interaction was succesful, else false</returns>
    public bool Interact(GameObject gameObject)
    {
        switch (curCustomerNeed.NeedName)
        {
            case NeedNameEnum.Empty:
                {
                    return false;
                }
            case NeedNameEnum.Hunger:
                {
                    SetNeed(NeedNameEnum.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case NeedNameEnum.Thirst:
                {
                    SetNeed(NeedNameEnum.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case NeedNameEnum.ALittlePainInTheLowerBack:
                {
                    SetNeed(NeedNameEnum.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case NeedNameEnum.AnUrgeToSpeakToTheManager:
                {
                    SetNeed(NeedNameEnum.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            default: return false;
        }
    }


    private IEnumerator WaitBeforeNextNeed(float time)
    {
        isWaiting = true;
        curCustomerNeed = new CustomerNeed(NeedNameEnum.Empty);
        aiController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(GetRandomEnum<NeedNameEnum>());
        isWaiting = false;
    }
}
