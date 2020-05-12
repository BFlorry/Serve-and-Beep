using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums.Pickupables;

public class TimedItemChanger : MonoBehaviour, IItemInteractable
{
    //Fields------------------------------------------------------------------
    [SerializeField]
    private bool multipleItemsAtATime;

    [Serializable]
    private class ItemPair
    {
        public ItemType itemType;
        public GameObject spawnItem;
        public float preparationTime;
        public AudioClip changeSound;
    }

    [SerializeField]
    private ItemPair[] itemPairs;

    private ItemSnap itemSnap;
    private PickupableManager pickupableManager;
    private GameObject curObj;
    private SoundManager soundManager;

    [SerializeField]
    private Image progressBarImage;
    [SerializeField]
    private GameObject progressBarCanvas;
    [SerializeField]
    private AudioClip cookingSfx;
    [SerializeField]
    private AudioClip removeSfx;

    //Methods-----------------------------------------------------------------
    private void Awake()
    {
        itemSnap = GetComponentInChildren<ItemSnap>();
        pickupableManager = FindObjectOfType<PickupableManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public bool Interact(GameObject obj)
    {
        if (obj.TryGetComponent(out Pickupable pickupable) && itemSnap.SnappedItem == null)
        {
            pickupable.Player.DropObject();
            itemSnap.SetToPoint(obj);
            StopAllCoroutines();
            StartCoroutine(ChangeItems(obj));
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider collider)
    {
        CheckIfStartChange(collider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckIfStartChange(collision.collider);
    }

    private void CheckIfStartChange(Collider collider)
    {
        Transform parent = collider.transform.parent;
        if (parent != null)
        {
            GameObject item = parent.gameObject;

            if (item.TryGetComponent(out Pickupable pickupable))
            {
                if ((pickupable.Carried == false && curObj == null) || multipleItemsAtATime == true)
                {
                    StopAllCoroutines();
                    StartCoroutine(ChangeItems(item));
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Transform parent = collider.transform.parent;
        if (parent != null)
        {
            if (parent.gameObject.Equals(curObj))
            {
                curObj = null;
                progressBarCanvas.SetActive(false);
            }
        }
    }

    private IEnumerator ChangeItems(GameObject originalObj)
    {
        if (originalObj.TryGetComponent(out Pickupable pickupable))
        {
            for (int i = 0; i < itemPairs.Length; i++)
            {
                if (pickupable.ItemType.Equals(itemPairs[i].itemType))
                {
                    curObj = originalObj;
                    for (int j = i; j < itemPairs.Length; j++)
                    {
                        //yield return new WaitForSeconds(itemPairs[j].preparationTime);
                        progressBarCanvas.SetActive(true);
                        float timeLeft = itemPairs[j].preparationTime;

                        while (timeLeft >= 0.0f)
                        {
                            timeLeft -= Time.deltaTime;
                            progressBarImage.fillAmount = (1 - timeLeft / itemPairs[j].preparationTime);
                            yield return null;
                        }

                        if ((itemSnap != null && itemSnap.SnappedItem != null) || itemSnap == null || multipleItemsAtATime)
                        {
                            if (itemPairs[j].changeSound != null)
                            {
                                soundManager.PlaySingle(itemPairs[j].changeSound);
                            }
                            pickupableManager.DespawnPickupable(curObj);
                            curObj = pickupableManager.SpawnPickupable(itemPairs[j].spawnItem, curObj.transform.position, curObj.transform.rotation);
                            
                        }
                    }
                    yield break;
                }
            }
        }
    }
}
