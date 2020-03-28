using System.Collections;
using UnityEngine;

/// <summary>
/// A class that contains and controls a customer's needs and areas of the needs.
/// </summary>
public class CustomerNeedController : MonoBehaviour, IItemInteractable
{
    //Fields----------------------------------------------------------------------

    CustomerNeed curNeed;

    public float currentValue;
    private float defaultValue = 0f;
    private float minWaitTime = 6f;
    private float maxWaitTime = 3f;

    CustomerNeedManager needManager;
    Customer customer;
    AIController aiController;
    CustomerAreas customerAreas;
    CustomerNeedDisplay display;
    private IEnumerator waitAfterMove;
    private bool isWaiting = false;

    //For testing and debugging only.
    private bool allowDebugCommands = false;

    //Methods---------------------------------------------------------------------

    /// <summary>
    /// Initializes customer need controller's attributes.
    /// </summary>
    void Start()
    {
        this.needManager = FindObjectOfType<CustomerNeedManager>();
        this.currentValue = defaultValue;
        this.customer = GetComponent<Customer>();
        this.aiController = GetComponent<AIController>();
        this.customerAreas = GetComponent<CustomerAreas>();
        this.display = GetComponent<CustomerNeedDisplay>();
        SetNeed(needManager.GetRandomNeed());
    }


    /// <summary>
    /// If customer has no need, sets it waiting for a new one.
    /// </summary>
    void Update()
    {
        /*
        if(curCustomerNeed.NeedName == NeedNameEnum.Empty && isWaiting == false)
        {
            StartCoroutine(WaitBeforeNextNeed(timeBetweenNeeds));
        }
        */    

        //For testing only
        SetNeedWithKeyboard();

        if (curNeed != null)
        {
            display.SetProgressDisplayValue(currentValue, curNeed.MaxValue, defaultValue);
        }
    }


    /// <summary>
    /// Sets current need. Sets AI controller's movement
    /// to the area where the need belongs to. Accepts null.
    /// </summary>
    /// <param name="need">A need to be set to this customer.</param>
    public void SetNeed(CustomerNeed need)
    {
        curNeed = need;
        display.SetNeedSprite(curNeed.Sprite);
        isWaiting = false;
        aiController.MoveToNeedDestination(need);
    }


    /// <summary>
    /// Method for testing. Sets need as the number key that was pressed.
    /// </summary>
    private void SetNeedWithKeyboard()
    {
        if (allowDebugCommands == true)
        {
            allowDebugCommands = false;
            if (int.TryParse(Input.inputString, out int number))
            {
                if (number >= 0 && number <= 9)
                {
                    if (number < needManager.Needs.Length)
                    {
                        SetNeed(needManager.Needs[number]);
                        Debug.Log("Customers' needs set as " + number + ". need.");
                    }
                    else
                    {
                        Debug.Log("Pressed key is not in area of needs.");
                    }
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    SetNeed(null);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D) &&
            Input.GetKeyDown(KeyCode.B) &&
            Input.GetKeyDown(KeyCode.G))
        {
            allowDebugCommands = true;
        }

    }


    /// <summary>
    /// Makes continuous changes to the state of customer's need.
    /// </summary>
    void FixedUpdate()
    {
        if (curNeed != null)
        {
            if (currentValue < curNeed.MaxValue)
            {
                currentValue += 0.05f; 
            }
            else
            {
                customer.SfGain(-curNeed.NegReview);
                StartCoroutine(WaitBeforeNextNeed(RandWaitTime()));
            }
        }
    }


    private float RandWaitTime()
    {
        return Random.Range(maxWaitTime, minWaitTime);
    }


    /// <summary>
    /// Accepts any interactable item and excecutes the customer's need completion.
    /// </summary>
    /// <param name="gameObject">interacting gameobject</param>
    /// <returns>true if interaction was succesful, else false</returns>
    public bool Interact(GameObject gameObject)
    {
        Pickupable pickupable = gameObject.GetComponent<Pickupable>();
        if (pickupable != null && curNeed.SatisfItem == pickupable.ItemType)
        {
            customer.SfGain(curNeed.PosReview);
            StartCoroutine(WaitBeforeNextNeed(RandWaitTime()));
            return true;
        }
        else
        {
            Debug.Log("Target is not pickupable. Pickupable component is null.");
            return false;
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
        display.SetNeedCanvasActivity(false);
        isWaiting = true;
        curNeed = null;
        aiController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(needManager.GetRandomNeed());
        isWaiting = false;
        display.SetNeedCanvasActivity(true);
    }
}
