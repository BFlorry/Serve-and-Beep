using System.Collections;
using UnityEngine;

/// <summary>
/// A class that contains and controls a customer's needs and areas of the needs.
/// </summary>
public class CustomerNeedController : MonoBehaviour, IItemInteractable
{
    //Fields----------------------------------------------------------------------

    private CustomerNeed curNeed;

    private float currentValue;
    private float defaultValue = 0f;
    private float minWaitTime = 3f;
    private float maxWaitTime = 6f;

    private CustomerNeedManager needManager;
    private Customer customer;
    private NavController navController;
    private CustomerNeedDisplay display;
    private IEnumerator waitAfterMove;

    //For testing and debugging only.
    private bool testCmdsActive = false;
    private KeyCode testCmdKey1 = KeyCode.F1;
    private KeyCode testCmdKey2 = KeyCode.F12;

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
        SetNeed(needManager.GetRandomNeed());
    }


    /// <summary>
    /// If customer has no need, sets it waiting for a new one.
    /// </summary>
    void Update()
    {
        if (curNeed != null)
        {
            display.SetProgressDisplayValue(currentValue, curNeed.MaxValue, defaultValue);
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
        curNeed = need;
        if (need != null)
        {
            display.SetNeedSprite(curNeed.Sprite);
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
        if (curNeed != null)
        {
            if (currentValue < curNeed.MaxValue)
            {
                currentValue += curNeed.DecreaseSpeed; 
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
        StartCoroutine(WaitBeforeNextNeed(RandWaitTime()));
    }

    public void ExitNeed()
    {
        StartCoroutine(WaitAndExit(RandWaitTime()));
        navController.SetReadyToExit();
    }

    private float RandWaitTime()
    {
        return Random.Range(minWaitTime, maxWaitTime);
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
            // TODO: If this change is final, remove this and following line
            //customer.SfGain(curNeed.PosReview);
            customer.ReviewPos();
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
        display.SetNeedCanvasActivity(false);
        curNeed = null;
        navController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(needManager.GetRandomNeed());
        display.SetNeedCanvasActivity(true); 
    }

    private IEnumerator WaitAndExit(float time)
    {
        display.SetNeedCanvasActivity(false);
        curNeed = null;
        navController.StopMovement();
        currentValue = defaultValue;
        yield return new WaitForSeconds(time);
        SetNeed(needManager.GetExitNeed());
        display.SetNeedCanvasActivity(true);
    }
}
