using UnityEngine;

public class CustomerPoint : MonoBehaviour
{
    //Fields----------------------------------------------------------------

     //Not necessary but may help at organization.
    [SerializeField]
    private string Name { get; }
    


    public bool IsOccupied { get; set; } = false;
    public Vector3 Position { get => this.gameObject.transform.position; }


}
