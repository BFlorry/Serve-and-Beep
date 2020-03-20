using UnityEngine;

/// <summary>
/// Class that contains areas in which customers can move.
/// </summary>
public class CustomerAreas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] areas = null;
    
    public GameObject[] Areas { get => areas; }
}
