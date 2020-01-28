using System;
using static EventHandlers.Delegates;

namespace EventHandlers
{

    public class DelegatesAndLambdas
    {
        public void CreateDelegate()
        {
            Delegates delegateExpl = new Delegates();
            IntOperation addOperation = new IntOperation(delegateExpl.Add);
            Console.WriteLine(addOperation(5, 5));
            IntOperation subtractOperation = new IntOperation(delegateExpl.Substract);
            Console.WriteLine(subtractOperation(5, 5));
        }

        public void Lambda()
        {
            IntOperation addOperation = (a, b) => a + b;
            IntOperation subOperation = (a, b) => a - b;
            Console.WriteLine(addOperation(5, 5));
            Console.WriteLine(subOperation(5, 5));
        }

        public void Closure()
        {
            ClosureExample ce = new ClosureExample();
            ce.SetLocalInt();
            Console.WriteLine($"Value from closure {ce.GetLocalInt()}");
        }

    }

    public class ClosureExample
    {
        public delegate int GetValue();
        public GetValue GetLocalInt;

        /// <summary>
        /// In this Closure example we can see that the compiler does not
        /// destroy the localInt variable, instead it's keeping it
        /// for the later use of the lambda expression
        /// </summary>
        public void SetLocalInt()
        {
            int localInt = 99;
            GetLocalInt = () => localInt;
        }


    }
    public class Delegates
    {
        public delegate int IntOperation(int a, int b);
        public int Add(int a, int b)
        {
            Console.WriteLine("Add called");
            return a + b;
        }
        public int Substract(int a, int b)
        {
            Console.WriteLine("Substract called");
            return a - b;
        }
    }
}
