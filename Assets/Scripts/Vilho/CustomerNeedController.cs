using System.Collections;
using UnityEngine;
using static Enums.Pickupables;

/// <summary>
/// A class that contains and controls a customer's needs and areas of the needs.
/// </summary>
public class CustomerNeedController : MonoBehaviour, IItemInteractable
{
    //Fields----------------------------------------------------------------------

    [SerializeField]
    private AudioClip interactSfx;
    [SerializeField]
    private float needObjDestroyTime = 0f;
    [SerializeField]
    private float minWaitTime = 3f;
    [SerializeField]
    private float maxWaitTime = 10f;

    private CustomerNeedManager needManager;
    private Customer customer;
    private NavController navController;
    private CustomerNeedDisplay display;
    private CustomerController customerController;

    private float currentValue;
    private float defaultValue = 0f;
    private bool needActive = false;

    //For testing and debugging only.
    private bool testCmdsActive = false;
    private KeyCode testCmdKey1 = KeyCode.F1;
    private KeyCode testCmdKey2 = KeyCode.F12;

    //Properties------------------------------------------------------------------
    public CustomerNeed CurNeed { get; private set; }

    //Methods---------------------------------------------------------------------

    /// <summary>
    /// Initializes customer need controller's attributes.
    /// </summary>
    void Start()
    {
        this.needManager = FindObjectOfType<CustomerNeedManager>();
        this.currentValue = defaultValue;
        this.customer = GetComponent<Customer>();
        this.navController = GetComponent<NavController>();
        this.display = GetComponent<CustomerNeedDisplay>();
        TryGetComponent(out customerController);
        SetNeed(needManager.GetRandomNeed());
        SetNeedActivity(false);
    }


    /// <summary>
    /// If customer has no need, sets it waiting for a new one.
    /// </summary>
    void Update()
    {
        if (CurNeed != null)
        {
            display.SetProgressDisplayValue(currentValue, CurNeed.MaxValue, defaultValue);
        }

        //For testing only
        SetNeedWithKeyboard();
    }


    /// <summary>
    /// Sets current need. Sets AI controller's movement
    /// to the area where the need belongs to. Accepts null.
    /// </summary>
    /// <param name="need">A need to be set to this customer.</param>
    public void SetNeed(CustomerNeed need)
    {
        CurNeed = need;
        if (need != null)
        {
            display.SetNeedSprite(CurNeed.Sprite);
            Debug.Log("Customers needs set as " + need.gameObject.name);
        }
        else
        {
            Debug.Log("Customers need set as null.");
        }
        navController.MoveToNeedDestination(need);
    }


    /// <summary>
    /// Method for testing. Sets need as the number key that was pressed.
    /// </summary>
    private void SetNeedWithKeyboard()
    {
        if (Input.GetKey(testCmdKey1) && Input.GetKey(testCmdKey2))
        {
            if (testCmdsActive == true)
            {
                testCmdsActive = false;
                Debug.Log("Test commands unactivated.");
            }
            else
            {
                testCmdsActive = true;
                Debug.Log("Test commands activated. Next key pressed acts as a command, if it has one defined." + "\n" +
                    "Unactivate test commands by pressing. " + testCmdKey1 + " and " + testCmdKey2 + "simultaneously.");
            }
        }

        if (testCmdsActive == true)
        {
            if (int.TryParse(Input.inputString, out int number))
            {
                if (number >= 1 && number <= 9)
                {
                    if (number <= needManager.Needs.Length)
                    {
                        int needInt = number - 1;
                        CustomerNeed need = needManager.Needs[needInt];
                        SetNeed(need);
                        currentValue = defaultValue;
                    }
                    else
                    {
                        Debug.Log("Pressed number is not in area of needs. There are " +
                            needManager.Needs.Length + " needs.");
                    }
                }
                else if (number == 0)
                {
                    SetNeed(null);
                }
            }
        }


    }

    /// <summary>
    /// Makes continuous changes to the state of customer's need.
    /// </summary>
    void FixedUpdate()
    {
        if (CurNeed != null && needActive)
        {
            if (currentValue < CurNeed.MaxValue)
            {
                currentValue += CurNeed.DecreaseSpeed; 
            }
            else
            {
                // TODO: If this change is final, remove this and following line
                //customer.SfGain(-curNeed.NegReview);
                customer.ReviewNeg();
                NextNeed();
            }
        }
    }

    private void NextNeed()
    {
        if(customerController.TimeToExit()) StartCoroutine(WaitAndExit(RandWaitTime()));
        else
        {
            StartCoroutine(WaitBeforeNextNeed(RandWaitTime()));
            customerController.RaiseNeed();
        }
    }

    private float RandWaitTime()
    {
        return Random.Range(minWaitTime, maxWaitTime);
    }

    public void SetNeedActivity(bool b)
    {
        display.SetNeedCanvasActivity(b);
        needActive = b;
    }

    public bool ItemTypeEquals(ItemType itemType)
    {
        if (CurNeed != null)
        {
            return CurNeed.ItemTypeEquals(itemType);
        }
        return false;
    }

    /// <summary>
    /// Accepts any interactable item and excecutes the customer's need completion.
    /// </summary>
    /// <param name="gameObject">interacting gameobject</param>
    /// <returns>true if interaction was succesful, else false</returns>
    public bool Interact(GameObject gameObject)
    {
        Pickupable pickupable = gameObject.GetComponent<Pickupable>();
        if (pickupable != null && CurNeed.SatisfItem == pickupable.ItemType)
        {
            pickupable.Player.GetComponent<PlayerSfxManager>().PlaySingle(interactSfx);
            pickupable.DropObjFromPlayer();
            pickupable.DestroyPickupable(needObjDestroyTime);

            // TODO: If this change is final, remove this and following line
            //customer.SfGain(curNeed.PosReview);
            customer.ReviewPos();
            customerController.PlayEatAnimation();
            NextNeed();
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
        SetNeedActivity(false);
        CurNeed = null;
        navController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(needManager.GetRandomNeed());
    }

    private IEnumerator WaitAndExit(float time)
    {
        SetNeedActivity(false);
        CurNeed = null;
        navController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(needManager.GetExitNeed());
        StartCoroutine(CheckForExitCondition());
    }

    private IEnumerator CheckForExitCondition()
    {
        while(navController.Moving == true)
        {
            yield return null;
        }
        FindObjectOfType<CustomerSpawner>().DespawnCustomer(this.gameObject);
    }
}
