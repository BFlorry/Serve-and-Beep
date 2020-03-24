using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Enums.CustomerEnums;

/// <summary>
/// A class that contains and controls a customer's needs and areas of the needs.
/// </summary>
public class CustomerNeedController : MonoBehaviour, IItemInteractable
{
    //Fields----------------------------------------------------------------------

    CustomerNeed curCustomerNeed = new CustomerNeed(NeedNameEnum.Empty, null);

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
    CustomerNeedDisplay display;
    private IEnumerator waitAfterMove;
    private bool isWaiting = false;

    private Sprite[] needImages;

    //Properties------------------------------------------------------------------

    public float MaxValue { get => maxValue; }
    public float CurrentValue { get => currentValue; }
    public float DefaultValue { get => defaultValue; }


    //Methods---------------------------------------------------------------------

    /// <summary>
    /// Initializes customer need controller's attributes.
    /// </summary>
    void Start()
    {
        this.currentValue = defaultValue;
        this.customer = GetComponent<Customer>();
        this.aiController = GetComponent<AIController>();
        this.customerAreas = GetComponent<CustomerAreas>();
        this.display = GetComponent<CustomerNeedDisplay>();
        this.needImages = FindObjectOfType<AIManager>().GetImages();
        SetNeed(GetRandomEnum<NeedNameEnum>());
    }


    /// <summary>
    /// If customer has no need, sets it waiting for a new one.
    /// </summary>
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
    /// Sets current need. Sets AI controller's movement
    /// to the area where the need belongs to.
    /// </summary>
    /// <param name="need">A need to be set to this customer.</param>
    public void SetNeed(NeedNameEnum needName)
    {
        //TODO: Image to customer need, not in this class
        if (needName != NeedNameEnum.Empty)
        {
            Sprite needImg = needImages[(int)needName-1];
            curCustomerNeed = new CustomerNeed(needName, needImg);
        }
        else
        {
            curCustomerNeed = new CustomerNeed(needName, null);
        }
        display.SetNeedImage(curCustomerNeed.Image);
        aiController.MoveToNeedDestination(curCustomerNeed);
    }


    /// <summary>
    /// Method for testing. Sets need as the number key that was pressed.
    /// </summary>
    private void SetNeedWithKeyboard()
    {
        if (int.TryParse(Input.inputString, out int number))
        {
            if (number >= 0 && number <= 9)
            {
                SetNeed((NeedNameEnum)number);
            }
        }
    }


    /// <summary>
    /// Makes continuous changes to the state of customer's need.
    /// </summary>
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


    /// <summary>
    /// Resets customer's need and waits for given time.
    /// Sets new random need.
    /// </summary>
    /// <param name="time">wait time as seconds</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator WaitBeforeNextNeed(float time)
    {
        isWaiting = true;
        curCustomerNeed = new CustomerNeed(NeedNameEnum.Empty, null);
        aiController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(GetRandomEnum<NeedNameEnum>());
        isWaiting = false;
    }
}
