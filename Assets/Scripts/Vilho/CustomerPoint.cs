using UnityEngine;

/// <summary>
/// Class for points that can act as customer's destinations.
/// Main reason of this class is to have occupation boolean for points.
/// </summary>
public class CustomerPoint : MonoBehaviour
{
    //Properties--------------------------------------------------------------

    public bool IsOccupied { get; set; } = false;
    public Vector3 Position { get => this.gameObject.transform.position; }
}
