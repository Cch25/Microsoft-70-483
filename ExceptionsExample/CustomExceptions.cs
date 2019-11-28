using System;

namespace ExceptionsExample
{
    public class CustomExceptions
    {
        public void MyCustomException()
        {
            try
            {
                throw new MyCustomException("Something went wrong", MyErrorCodes.DivideByZero);
            }
            catch (MyCustomException me) when (me.Error == MyErrorCodes.InvalidNumberText)
            {//if the when is not matched, then the program will be terminated.
                Console.WriteLine(me.Error);
                Console.WriteLine(me.Message);
            }
            catch(MyCustomException me) when (me.Error == MyErrorCodes.DivideByZero)
            {
                Console.WriteLine("Cannot devide by zero");
            }
        }
    }


    public class MyCustomException : Exception
    {
        public MyErrorCodes Error { get; set; }
        public MyCustomException(string message, MyErrorCodes myErrorCodes) : base(message)
        {
            Error = myErrorCodes;
        }

    }

    public enum MyErrorCodes
    {
        InvalidNumberText,
        DivideByZero
    }
}
