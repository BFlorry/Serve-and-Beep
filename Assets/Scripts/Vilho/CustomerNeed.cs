using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums.Pickupables;

public class CustomerNeed : MonoBehaviour
{
    //Fields--------------------------------------------------------------

    [SerializeField]
    private ItemType satisfItem;

    [SerializeField]
    private int tokenAmount;
    [SerializeField]
    private int posReview;
    [SerializeField]
    private int negReview;
    [SerializeField]
    private int decreaseSpeed;
    [SerializeField]
    private float maxValue;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private GameObject area;
    [SerializeField]
    private GameObject pointGroup;


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
