using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAlt : MonoBehaviour
{
    Rigidbody2D rb;
    private float dist = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Renderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 3;
        if (Input.GetButton("Fire")) ActionButton();
    }

    void ActionButton()
    {
        bool foundHit = Physics2D.Raycast(transform.position, transform.right, dist, 1 << 8);
        //Debug.DrawRay(transform.position, new Vector2(x, y), Color.green);
    }
}
