using System;
using System.Collections;
using System.Collections.Generic;

namespace covariance_contravariance
{
    class Program
    {
        public delegate L1 SampleDelegate(L3 t);

        public delegate TResult SampleGenericDelegate<in TP, out TResult>(TP a);
        public delegate T SampleGenericDelegate1<out T>();
        delegate void Action<in T>(T t);
        delegate TResult Func<out TResult>();

        static void Main(string[] args)
        {
            // Assignment compatibility
            // 赋值兼容
            string str = "test";

            // An object of a more derived type is assigned to an object of a less derived type.
            // 派生程度较大的类型可以赋值给派生程度较小的类型
            object obj = str;

            object[] strs = new string[] { "a", "b", "c" };
            strs[0] = 3;// ArrayTypeMismatchException exception

            List<Giraffe> giraffes = new List<Giraffe>();
            giraffes.Add(new Giraffe());
            //List<Animal> animals = giraffes;
            //animals.Add(new Lion()); // Aargh!

            // Assigning a method with a matching signature
            // to a non-generic delegate. No conversion is necessary.
            // 将签名完全匹配的方法赋值给委托，无需转换
            SampleDelegate d1 = RL1PL3;

            // Assigning a method with a more derived return type
            // and less derived argument type to a non-generic delegate.
            // The implicit conversion is used.
            // 将一个返回值的派生程度更大（协变），参数值的派生程度更小（逆变）的方法赋值给委托
            SampleDelegate d2 = RL2PL2;

            // 方法返回值派生程度继续变大，参数值的派生程度继续变小
            SampleDelegate dNonGenericConversion1 = RL3PL1;

            // 一个委托就可以适用于全部返回值、参数组合
            // 使代码更通用
            SampleDelegate sd1 = RL1PL1;
            SampleDelegate sd2 = RL1PL2;
            SampleDelegate sd3 = RL1PL3;
            SampleDelegate sd4 = RL2PL1;
            SampleDelegate sd5 = RL2PL2;
            SampleDelegate sd6 = RL2PL3;
            SampleDelegate sd7 = RL3PL1;
            SampleDelegate sd8 = RL3PL2;
            SampleDelegate sd9 = RL3PL3;

            // 泛型委托类似：
            SampleGenericDelegate<L3, L1> dg1 = RL1PL1;
            SampleGenericDelegate<L3, L1> dg2 = RL1PL2;
            SampleGenericDelegate<L3, L1> dg3 = RL1PL3;
            SampleGenericDelegate<L3, L1> dg4 = RL2PL1;
            SampleGenericDelegate<L3, L1> dg5 = RL2PL2;
            SampleGenericDelegate<L3, L1> dg6 = RL2PL3;
            SampleGenericDelegate<L3, L1> dg7 = RL3PL1;
            SampleGenericDelegate<L3, L1> dg8 = RL3PL2;
            SampleGenericDelegate<L3, L1> dg9 = RL3PL3;

            SampleGenericDelegate1<string> dString = () => " ";

            SampleGenericDelegate1<object> dObject1 = () => " ";

            // The following statement generates a compiler error  
            // 如果不使用 out 显示指明委托的结果支持逆变，下面的一行代码无法编译通过 
            SampleGenericDelegate1 <object> dObject = dString;

            IEnumerable<string> listL3 = new List<string>();
            IEnumerable<object> lis = listL3;

            // 协变
            var employees = new List<Teacher>();
            PrintFullName(employees);

            // 协变
            var students = new List<Student>();
            PrintFullName(students);

            // 协变 逆变只适用于引用类型
            IEnumerable<int> listOfInt = new List<int>();
            //IEnumerable<object> lo = listOfInt;

            // You can pass ShapeAreaComparer, which implements IComparer<Shape>,
            // even though the constructor for SortedSet<Circle> expects 
            // IComparer<Circle>, because type parameter T of IComparer<T> is
            // contravariant.
            // SortedSet<Circle> 的构造函数需要IComparer<Circle>，但仍然可以传入是实现IComparer<Shape>的ShapeAreaComparer
            SortedSet<Circle> circlesByArea =
                new SortedSet<Circle>(new ShapeAreaComparer())
                    { new Circle(7.2), new Circle(100), null, new Circle(.01) };

            foreach (Circle c in circlesByArea)
            {
                Console.WriteLine(c == null ? "null" : "Circle with area " + c.Area);
            }
        }

        public static L1 RL1PL1(L1 b)
        { return new L1(); }

        public static L1 RL1PL2(L2 d)
        { return new L1(); }

        public static L1 RL1PL3(L3 d)
        { return new L1(); }

        public static L1 RL2PL1(L2 d)
        { return new L1(); }

        public static L2 RL2PL2(L1 b)
        { return new L2(); }

        public static L2 RL2PL3(L2 d)
        { return new L3(); }

        public static L3 RL3PL1(L1 f)
        { return new L3(); }

        public static L3 RL3PL2(L2 f)
        { return new L3(); }

        public static L3 RL3PL3(L2 f)
        { return new L3(); }

        public static void PrintFullName(IEnumerable<Person> persons)
        {

        }
    }

    public abstract class Person
    {
    }
    public class Teacher: Person
    {
    }
    
    public class Student: Person
    {
    }

    internal class Lion : Animal
    {
    }

    internal class Animal
    {
    }

    internal class Giraffe:Animal
    {
    }

    public class L1 { }

    public class L2 : L1 { }

    public class L3 : L2 { }

    abstract class Shape
    {
        public virtual double Area { get { return 0; } }
    }

    class Circle : Shape
    {
        private double r;

        public Circle(double radius) { r = radius; }

        public double Radius { get { return r; } }

        public override double Area { get { return Math.PI * r * r; } }
    }

    class ShapeAreaComparer : IComparer<Shape>
    {
        int IComparer<Shape>.Compare(Shape a, Shape b)
        {
            if (a == null) return b == null ? 0 : -1;
            return b == null ? 1 : a.Area.CompareTo(b.Area);
        }
    }
}
