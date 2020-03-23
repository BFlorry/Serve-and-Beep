using UnityEngine;

/// <summary>
/// Class that contains gameobjects that act as cordinate areas.
/// </summary>
public class CustomerAreas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] areas = null;

    public Bounds[] Bounds { get; private set; } = null;

    private void Awake()
    {
        Bounds = new Bounds[areas.Length];
        for (int i = 0; i < Bounds.Length; i++)
        {
            Bounds[i] = areas[i].GetComponent<MeshRenderer>().bounds;
        }
    }
}
