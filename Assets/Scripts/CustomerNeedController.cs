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
    CustomerNeed curNeed = new CustomerNeed(Name.Empty);

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


    /// <summary>
    /// Sets this customer's current need as given need.
    /// Gets area that the given need belongs to.
    /// Sets ai controller's movement to that area.
    /// </summary>
    /// <param name="need">A need to be set to this customer.</param>
    public void SetNeed(Name name)
    {
        curNeed = new CustomerNeed(name);
        aiController.MoveTo(curNeed.Area);
    }


    /// <summary>
    /// Method for testing. Sets need as the number key that was pressed.
    /// </summary>
    private void SetNeedWithKeyboard()
    {
        int input = int.Parse(Input.inputString);
        if (input >= 0 && input <= 9)
        {
            CustomerNeed need = new CustomerNeed((Name)input);
            SetNeed(need);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        currentValue = defaultValue;
        customer = GetComponent<Customer>();
        aiController = GetComponent<AIController>();
        customerAreas = GetComponent <CustomerAreas>();
        CustomerNeed need = new CustomerNeed(GetRandomEnum<Name>());
        SetNeed(need);
    }


    void Update()
    {
        if(curNeed.Need == Name.Empty && isWaiting == false)
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
        if(curNeed.Need == Name.Hunger)
        {
            if (currentValue < maxValue) currentValue += 0.05f;
            else
            {
                SetNeed(Name.Empty);
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
        switch (curNeed.Need)
        {
            case Name.Empty:
                {
                    return false;
                }
            case Name.Hunger:
                {
                    SetNeed(Name.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case Name.Thirst:
                {
                    SetNeed(Name.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case Name.ALittlePainInTheLowerBack:
                {
                    SetNeed(Name.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            case Name.AnUrgeToSpeakToTheManager:
                {
                    SetNeed(Name.Empty);
                    customer.SfGain(satisfactionModifier);
                    return true;
                }
            default: return false;
        }
    }


    private IEnumerator WaitBeforeNext(float time)
    {
        this.curNeed = Name.Empty;
        aiController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(GetRandomNeed());
        isWaiting = false;
    }
}
