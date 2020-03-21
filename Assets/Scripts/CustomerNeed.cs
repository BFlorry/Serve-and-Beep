using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A class that contains and controls a customer's needs and the needs' areas.
/// </summary>
public class CustomerNeed : MonoBehaviour, IItemInteractable
{
    //Fields (enums later) ------------------------------------------------------------------

    [SerializeField]
    Need curNeed = Need.Empty;
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


    //Enums---------------------------------------------------------------------

    public enum Need
    {
        Empty,
        Hunger,
        Thirst,
        ALittlePainInTheLowerBack,
        AnUrgeToSpeakToTheManager
    }

    /// <summary>
    /// Enum number equals area's id.
    /// </summary>
    public enum Area
    {
        Empty = 0,
        Restaurant = 1,
        Bar = 2,
        ThaiMassage = 3,
        Info = 4
    }


    //Methods--------------------------------------------------------------------

    /// <summary>
    /// Defines which need belongs to which area.
    /// </summary>
    /// <param name="need">a need</param>
    /// <returns>area of given need</returns>
    private Area GetArea(Need need)
    {
        switch (need)
        {
            case Need.Empty: return Area.Empty;
            case Need.Hunger: return Area.Restaurant;
            case Need.Thirst: return Area.Bar;
            case Need.ALittlePainInTheLowerBack: return Area.ThaiMassage;
            case Need.AnUrgeToSpeakToTheManager: return Area.Info;
            default: return Area.Empty;
        }
    }


    /// <summary>
    /// Sets this customer's current need as given need.
    /// Gets area that the given need belongs to.
    /// Sets ai controller's movement to that area.
    /// </summary>
    /// <param name="need">A need to be set to this customer.</param>
    public void SetNeed(Need need)
    {
        curNeed = need;
        int areaId = (int)GetArea(need);
        aiController.MoveToArea(areaId);
    }


    /// <summary>
    /// Method for testing. Sets need as the number key that was pressed.
    /// </summary>
    private void SetNeedWithKeyboard()
    {
        int input = int.Parse(Input.inputString);
        if (input >= 0 && input <= 9)
        {
            SetNeed((Need)input);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        currentValue = defaultValue;
        customer = GetComponent<Customer>();
        aiController = GetComponent<AIController>();
        customerAreas = GetComponent <CustomerAreas>();
        SetNeed(GetRandomNeed());
    }


    void Update()
    {
        if(curNeed == Need.Empty && isWaiting == false)
        {
            isWaiting = true;
            StartCoroutine(WaitBeforeNext(timeBetweenNeeds));
        }
        
        //For testing only
        SetNeedWithKeyboard();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(curNeed == Need.Hunger)
        {
            if (currentValue < maxValue) currentValue += 0.05f;
            else
            {
                SetNeed(Need.Empty);
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
        switch (curNeed)
        {
            case Need.Empty:
                {
                    return false;
                }
            case Need.Hunger:
                {
                    SetNeed(Need.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case Need.Thirst:
                {
                    SetNeed(Need.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case Need.ALittlePainInTheLowerBack:
                {
                    SetNeed(Need.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case Need.AnUrgeToSpeakToTheManager:
                {
                    SetNeed(Need.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            default: return false;
        }
    }


    private IEnumerator WaitBeforeNext(float time)
    {
        this.curNeed = Need.Empty;
        aiController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(GetRandomNeed());
        isWaiting = false;
    }


    /// <summary>
    /// Gets all needs and returns one randomly.
    /// </summary>
    /// <returns>random need</returns>
    public Need GetRandomNeed()
    {
        Array needs = Enum.GetValues(typeof(Need));
        int needInt = UnityEngine.Random.Range(0, needs.Length);
        object needObj = needs.GetValue(needInt);
        return (Need)needObj;
    }
}
