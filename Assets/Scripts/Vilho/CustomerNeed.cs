using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums.Pickupables;

public class CustomerNeed : MonoBehaviour
{
    //Fields--------------------------------------------------------------

    [SerializeField]
    public ItemType satisfItem;

    [SerializeField]
    public int tokenAmount;
    [SerializeField]
    public int posReview;
    [SerializeField]
    public int negReview;
    [SerializeField]
    public int decreaseSpeed;
    [SerializeField]
    public float maxValue;

    [SerializeField]
    public Sprite sprite;

    [SerializeField]
    public GameObject area;
    [SerializeField]
    public GameObject pointGroup;


    //Properties---------------------------------------------------------

    public ItemType SatisfItem { get => satisfItem; }
    public int TokenAmount { get => tokenAmount; }
    public int PosReview { get => posReview; }
    public int NegReview { get => negReview; }
    public int DecreaseSpeed { get => decreaseSpeed; }
    public float MaxValue { get => maxValue; }
    public Sprite Sprite { get => sprite; }
    public GameObject Area { get => area; }
    public GameObject PointGroup { get => pointGroup; }
}
