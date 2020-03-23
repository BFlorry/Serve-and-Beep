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

    /// <summary>
    /// A class for containing customer enums.
    /// </summary>
    public static class CustomerEnums
    {
        //Fields--------------------------------------------------------------------

        /// <summary>
        /// Possible needs of a customer.
        /// </summary>
        public enum NeedNameEnum
        {
            Empty,
            Hunger,
            Thirst,
            ALittlePainInTheLowerBack,
            AnUrgeToSpeakToTheManager
        }

        /// <summary>
        /// An area where the customer goes, when having a need.
        /// </summary>
        public enum AreaEnum
        {
            Empty = 0,
            Restaurant = 1,
            Bar = 2,
            ThaiMassage = 3,
            Info = 4
        }

        /// <summary>
        /// A point where the customer goes, when having a need.
        /// </summary>
        public enum PointGroupEnum
        {
            Empty = 0,
            Restaurant = 1,
            Bar = 2,
            ThaiMassage = 3,
            Info = 4
        }


        //Methods-------------------------------------------------------------------

        /// <summary>
        /// Gets all enums of given type and returns one randomly.
        /// </summary>
        /// <typeparam name="T">some Enum type</typeparam>
        /// <returns>random enum of the given type</returns>
        public static T GetRandomEnum<T>() where T : Enum
        {
            return Enums.GetRandomEnum<T>();
        }
    }
}
