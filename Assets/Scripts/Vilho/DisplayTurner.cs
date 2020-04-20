using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps canvas facing camera.
/// </summary>
public class DisplayTurner : MonoBehaviour
{
    new private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.transform.LookAt(
                this.gameObject.transform.position + camera.transform.rotation * Vector3.forward,
                camera.transform.rotation * Vector3.up);
        }
    }
}
