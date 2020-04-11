using System;

/// <summary>
/// Class that contains nested enum classes and a method for getting random enum.
/// </summary>
public static class Enums
{
    //Methods--------------------------------------------------------

    /// <summary>
    /// Gets all enums of given type and returns one randomly.
    /// </summary>
    /// <typeparam name="T">some Enum type</typeparam>
    /// <returns>random enum of the given type</returns>
    public static T GetRandomEnum<T>() where T : Enum
    {
        Array enums = Enum.GetValues(typeof(T));
        int enumInt = UnityEngine.Random.Range(0, enums.Length);
        object enumObj = enums.GetValue(enumInt);
        return (T)enumObj;
    }


    //Nested classes-------------------------------------------------

    public static class Pickupables
    {
        //Fields--------------------------------------------------------------------

        [Flags] public enum ItemType
        {
            Empty,
            Food,
            Drink,
            RedDrink,
            BlueDrink,
            YellowDrink,
            OrangeDrink,
            PurpleDrink,
            GreenDrink,
            BlackDrink,
        }
    }
}
