using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_SimpleMove : MonoBehaviour {

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        GetComponent<Rigidbody2D>().velocity = targetVelocity * 5;
    }
}
