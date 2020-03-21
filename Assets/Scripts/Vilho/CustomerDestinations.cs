using UnityEngine;

/// <summary>
/// Class that contains gameobjects that act as cordinate areas or points.
/// </summary>
public class CustomerDestinations : MonoBehaviour
{
    [SerializeField]
    private GameObject[] destinations = null;
    
    public GameObject[] Destinations { get => destinations; }
}
