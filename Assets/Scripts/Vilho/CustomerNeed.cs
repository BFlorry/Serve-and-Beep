using static Enums.CustomerEnums;

public class CustomerNeed
{
    //Properties----------------------------------------------------------------

    public NeedNameEnum NeedName { get; } = NeedNameEnum.Empty;
    public AreaEnum Area { get; } = AreaEnum.Empty;
    public PointGroupEnum Point { get; } = PointGroupEnum.Empty;


    //Constructors--------------------------------------------------------------

    public CustomerNeed(NeedNameEnum need)
    {
        this.NeedName = need;

        switch (need)
        {
            case NeedNameEnum.Empty:
                {
                    break;
                }
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
            case NeedNameEnum.ALittlePainInTheLowerBack:
                {
                    this.Point = PointGroupEnum.ThaiMassage;
                    break;
                }
            case NeedNameEnum.AnUrgeToSpeakToTheManager:
                {
                    this.Point = PointGroupEnum.Info;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    public CustomerNeed(NeedNameEnum need, AreaEnum area)
    {
        this.NeedName = need;
        this.Area = area;
        this.Point = PointGroupEnum.Empty;
    }


    public CustomerNeed(NeedNameEnum need, PointGroupEnum point)
    {
        this.NeedName = need;
        this.Point = point;
        this.Area = AreaEnum.Empty;
    }
}
