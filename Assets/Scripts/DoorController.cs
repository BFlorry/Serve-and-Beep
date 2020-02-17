using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public OxygenController room1;
    public OxygenController room2;

    [SerializeField]
    private bool doorOpen = false;

    private float toRoom1 = 0;
    private float toRoom2 = 0;

    private float oxygenFlowRate = 0.3f;

    private Color doorClosedColor = new Color(1.0f, 0.64f, 0.0f);
    private Color doorOpenedColor = new Color(1, 1, 1);


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material.color = doorClosedColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doorOpen)
        {

            //if((room1.Oxygen != 100f || room2.Oxygen != 100f) && (room1.Oxygen != 0 || room2.Oxygen != 0))
            if (Mathf.Abs(room1.Oxygen - room2.Oxygen) >= 0.1)
            {
                Debug.Log("1");
                if (room1.Oxygen > room2.Oxygen)
                {
                    toRoom1 = (room2.Oxygen - room1.Oxygen) * oxygenFlowRate * Time.deltaTime;
                    toRoom2 = (room1.Oxygen - room2.Oxygen) * oxygenFlowRate * Time.deltaTime;
                    Debug.Log("2");
                }
                else
                {
                    toRoom1 = -(room1.Oxygen - room2.Oxygen) * oxygenFlowRate * Time.deltaTime;
                    toRoom2 = (room1.Oxygen - room2.Oxygen) * oxygenFlowRate * Time.deltaTime;
                    Debug.Log("3");
                }
                room1.Oxygen += toRoom1;
                room2.Oxygen += toRoom2;

                if (room1.Oxygen <= 0.1f) room1.Oxygen = 0;
                if (room2.Oxygen <= 0.1f) room2.Oxygen = 0;
                //if (room1.Oxygen <= 0.1f && toRoom1 < 0) room1.Oxygen = 0;
                //if (room1.Oxygen > 99f && toRoom1 > 0) room1.Oxygen = 100f;
                //if (room2.Oxygen <= 0.1f && toRoom2 < 0) room2.Oxygen = 0;
                //if (room2.Oxygen > 99f && toRoom2 > 0) room2.Oxygen = 100f;

                toRoom1 = toRoom2 = 0f;

                //  (oxygenRoomA - oxygenRoomB) * timeSinceLastUpdate * oxygenFlowPerSecond
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        this.GetComponent<Renderer>().material.color = doorOpenedColor;
        doorOpen = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        this.GetComponent<Renderer>().material.color = doorClosedColor;
        doorOpen = false;
    }
}
