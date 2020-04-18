using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums.Pickupables;

public class CustomerArea : MonoBehaviour
{
    [SerializeField]
    private List<ItemType> itemTypes = new List<ItemType>();

    private void Awake()
    {
        if (itemTypes.Count == 0 &&
            this.transform.parent.gameObject.TryGetComponent(out CustomerNeed need))
        {
            itemTypes.Add(need.SatisfItem);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out CustomerNeedController custNeedCtrl))
        {
            foreach (ItemType itemType in itemTypes)
            {
                if (custNeedCtrl.ItemTypeEquals(itemType))
                {
                    custNeedCtrl.SetNeedDisplayActivity(true);
                    return;
                }
            }
        }
    }
}
