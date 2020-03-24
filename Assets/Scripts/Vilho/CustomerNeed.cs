using UnityEngine;
using UnityEngine.UI;
using static Enums.CustomerEnums;

/// <summary>
/// Class that contains customr's need name, area and point group.
/// </summary>
public class CustomerNeed : MonoBehaviour
{
    //Properties----------------------------------------------------------------

    [SerializeField]
    public Sprite Image { get; private set; }

    public NeedNameEnum NeedName { get; } = NeedNameEnum.Empty;
    public AreaEnum Area { get; } = AreaEnum.Empty;
    public PointGroupEnum Point { get; } = PointGroupEnum.Empty;


    //Constructors--------------------------------------------------------------

    /// <summary>
    /// Default constructor that adds area or point
    /// group according to given name of need.
    /// </summary>
    /// <param name="needName">name of need</param>
    public CustomerNeed(NeedNameEnum needName, Sprite needImg)
    {
        this.Image = needImg;
        this.NeedName = needName;

        switch (needName)
        {
            case NeedNameEnum.Hunger:
                {
                    this.Area = AreaEnum.Restaurant;
                    break;
                }
            case NeedNameEnum.Thirst:
                {
                    this.Area = AreaEnum.Bar;
                    break;
                }
            case NeedNameEnum.NeedToPiss:
                {
                    this.Area = AreaEnum.Toilet;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    /// <summary>
    /// Constructor that makes it possible to create custom need
    /// that has different area than the default for same name.
    /// </summary>
    /// <param name="needName">name of the need</param>
    /// <param name="area">area of the need</param>
    public CustomerNeed(NeedNameEnum needName, AreaEnum area)
    {
        this.NeedName = needName;
        this.Area = area;
        this.Point = PointGroupEnum.Empty;
    }


    /// <summary>
    /// Constructor that makes it possible to create custom need
    /// that has different point group than the default for same name.
    /// </summary>
    /// <param name="needName">name of the need</param>
    /// <param name="pointGroup">area of the need</param>
    public CustomerNeed(NeedNameEnum needName, PointGroupEnum pointGroup)
    {
        this.NeedName = needName;
        this.Point = pointGroup;
        this.Area = AreaEnum.Empty;
    }
}
