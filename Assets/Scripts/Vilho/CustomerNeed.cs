using static Enums.CustomerEnums;

public class CustomerNeed
{
    //Properties----------------------------------------------------------------

    public Name Need { get; }
    public Area Area { get; }
    public Point Point { get; }


    //Constructors--------------------------------------------------------------

    public CustomerNeed(Name need)
    {
        this.Need = need;
        this.Area = Area.Empty;
    }


    public CustomerNeed(Name need, Area area)
    {
        this.Need = need;
        this.Area = area;
        this.Point = Point.Empty;
    }


    public CustomerNeed(Name need, Point point)
    {
        this.Need = need;
        this.Point = point;
        this.Area = Area.Empty;
    }
}
