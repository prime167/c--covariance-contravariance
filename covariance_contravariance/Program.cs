using System;
using System.Collections.Generic;

namespace covariance_contravariance
{
    class Program
    {
        delegate object Print();

        delegate void MammalState(Mammal a);

        static void Main(string[] args)
        {
            // Assignment compatibility.   
            string str = "test";

            // An object of a more derived type is assigned to an object of a less derived type.   
            object obj = str;

            // Covariance.   
            IEnumerable<string> strings = new List<string>();
            // An object that is instantiated with a more derived type argument   
            // is assigned to an object instantiated with a less derived type argument.   
            // Assignment compatibility is preserved.   
            IEnumerable<object> objects = strings;

            object[] strs = new string[] { "a", "b", "c" };
            strs[0] = 3;
            Animal[] anis = new Giraffe[] {new Giraffe(), new Giraffe(), };
            anis[0] = new Tiger(); // ArrayTypeMismatchException exception

            var print = new Print(PrintString);
            print();

            // Contravariance
            // Assume that the following method is in the class:   
            // static void SetObject(object o) { }   
            Action<object> actObject = SetObject;

            // An object that is instantiated with a less derived type argument   
            // is assigned to an object instantiated with a more derived type argument.   
            // Assignment compatibility is reversed.   
            Action<string> actString = actObject;
        }

        static void ProcessMammal(Mammal mammal)
        {

        }

        static void ProcessAnimal(Animal mammal)
        {

        }

        static void ProcessGiraffe(Giraffe giraffe)
        {

        }

        private static void SetObject(object obj)
        {

        }

        static string PrintString()
        {
            Console.WriteLine("42");
            return "42";
        }
    }

    internal class Tiger : Mammal
    {
    }

    internal class Giraffe : Mammal
    {
    }

    internal class Animal
    {
    }

    internal class Mammal : Animal
    {
    }
}
