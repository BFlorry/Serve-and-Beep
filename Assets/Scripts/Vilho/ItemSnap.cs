using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSnap : MonoBehaviour
{
    //Fields------------------------------------------------------------
    [SerializeField]
    private AudioClip snapSound;

    private Vector3 point;
    private SoundManager soundManager;

    //Properties--------------------------------------------------------
    public GameObject SnappedItem { get; set; }

    //Methods-----------------------------------------------------------
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        GameObject pointObj = transform.GetChild(0).gameObject;
        if (pointObj != null)
        {
            point = pointObj.transform.position;
        }
        else
        {
            point = this.transform.position;
            Debug.LogWarning("Snap point not found.");
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        Transform parent = collider.transform.parent;
        if (parent != null)
        {
            GameObject item = parent.gameObject;
            SetToPoint(item);
        }
    }

    public void SetToPoint(GameObject item)
    {
        if (SnappedItem == null)
        {
            if (item.TryGetComponent(out Pickupable pickupable))
            {
                if (pickupable.Carried == false && pickupable.ItemSnap == null)
                {
                    pickupable.ItemSnap = this;
                    item.transform.position = point;
                    item.GetComponent<Rigidbody>().isKinematic = true;
                    SnappedItem = item;
                    soundManager.PlaySingle(snapSound);
                }
            }
        }
    }
}
