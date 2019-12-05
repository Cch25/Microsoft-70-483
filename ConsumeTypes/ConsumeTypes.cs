using System;

namespace ConsumeType
{
    public class ConsumeTypes
    {
        public void BoxingAndUnboxing()
        {
            object o = 99; //boxing - reference type.
            int value = (int) o; // unboxing - value type.
            float x = 9.9f; 
            int y = (int)x;//explicit conversion is required here
            Console.WriteLine(value);
            Console.WriteLine(y);
        }

        public void ImplicitExplicitOperator()
        {
            #region [ Book example ]
            Miles miles = new Miles(100);
            Kilometers k = miles; // implicit conversion
            Console.WriteLine(k.Distance);
            int intMiles = (int)miles;// explicit conversion 
            Console.WriteLine($"{intMiles}"); 
            #endregion

            #region [ Custom conversion ]
            House house = new House()
            {
                Location = "New York",
                Rooms = 4
            };
            Apartment apartment = (Apartment)house; //explicit conversion
            Villa villa = house; //implicit conversion
            Console.WriteLine($"Apartment conversion { apartment.Placement}");
            Console.WriteLine($"Villa conversion { villa.Placement} -Rooms {villa.Rooms}"); 
            #endregion
        }

        public void UsingDynamic()
        {
            dynamic d = 99; //var/let from javascript basically :)
            d += 1;
            Console.WriteLine(d);
            d = "Hi, ";
            d += "Cch25!";
            Console.WriteLine(d);
        }

    }
    internal class Miles
    {
        public double Distance { get; }
        public Miles(double miles)
        {
            Distance = miles;
        }
        public static implicit operator Kilometers(Miles t)
        {
            Console.WriteLine("Implicit conversion from miles to kilometers");
            return new Kilometers(t.Distance * 1.6);
        }
        public static explicit operator int(Miles t)
        {
            Console.WriteLine("Explicit conversion from miles to int");
            return (int)(t.Distance + 0.5);
        }
    }
    internal class Kilometers
    {
        public double Distance { get; set; }
        public Kilometers(double kilometers)
        {
            Distance = kilometers;
        }


    }
    internal class House
    {
        public string Location { get; set; }
        public int Rooms { get; set; }
    }
    internal class Apartment
    {
        public string Placement { get; set; }

        public static explicit operator Apartment(House house)
        {
            return new Apartment { Placement = house.Location};
        }
    }
    internal class Villa
    {
        public string Placement { get; set; }
        public int Rooms { get; set; }
        public static implicit operator Villa(House house)
        {
            return new Villa() { Placement = house.Location, Rooms = house.Rooms };
        }
    }
}
