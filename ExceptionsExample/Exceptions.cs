using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExceptionsExample
{
    public class Exceptions
    {
        public void ReadNumbersFromUser()
        {
            try
            {
                Console.WriteLine("Enter a number: ");
                string number = Console.ReadLine();
                int result = int.Parse(number);
                Console.WriteLine($"You've enterd the number {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine($"Error Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"Error HelpLink: {ex.HelpLink}");
                Console.WriteLine($"Error Target site: {ex.TargetSite}");
                Console.WriteLine($"Error Source: {ex.Source}");
            }
        }

        /// <summary>
        /// If Environment.FailFast is present, then the finally block will NOT
        /// get executed anymore, the program will immediately terminate.
        /// </summary>
        public void ReadNumberDivideByZeroOrWrongFormat()
        {
            try
            {
                Console.WriteLine("Enter a number: ");
                string number = Console.ReadLine();
                int result = int.Parse(number);
                result = 1 / result;
                Console.WriteLine($"You've enterd the number {result}");
            }
            catch (NotFiniteNumberException nfne)
            {
                Console.WriteLine(nfne.Message);
                Environment.FailFast("Critical error");
            }
            catch (DivideByZeroException dbze)
            {
                Console.WriteLine(dbze.Message);
                Environment.FailFast("Critical error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine($"Error Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"Error HelpLink: {ex.HelpLink}");
                Console.WriteLine($"Error Target site: {ex.TargetSite}");
                Console.WriteLine($"Error Source: {ex.Source}");
                Environment.FailFast("Critical error");
            }
            finally
            {
                Console.WriteLine("Thanks for using me");
            }
        }

        /// <summary>
        /// We can also pass as a secondary parameter to throw new Exeption("message",ex.Message");
        /// This kind of exeption can be the innerExeption, which can actually be used to throw the exeption.
        /// </summary>
        public void ThrowYourOwnExceptionWithInnerException()
        {
            try
            {
                throw new Exception("I'm a weird exception", new Exception("I'm an inner exception"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, ex);
            }
        }

        /// <summary>
        /// Propagate exeption by using throw instead of th throw ex.
        /// Throw ex will delete your stack trace, meaning that it will show you where the error it catch the error.
        /// Instead throw will show you the actual stack trace, meaning that it will show you the right line were something went wrong.
        /// This throw is quite cool and it will help you alot in your debugging experience :)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public void CatchExceptionFromAnotherMethod()
        {
            try
            {
                Divide(5, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("------------");
                Console.WriteLine(ex.Message);

            }
        }

        private int Divide(int number, int divisor)
        {
            try
            {
                return number / divisor;
            }
            catch (DivideByZeroException)
            {
                //TODO: log error
                Console.WriteLine("Can't divide by 0");
                throw;//propage this error
            }

        }

        /// <summary>
        /// We can pass our inner exception to outer exception to be read along with the stacktrace
        /// </summary>
        public void HandlingInnerExceptions()
        {
            try
            {
                try
                {
                    Console.WriteLine("Please input a number");
                    string result = Console.ReadLine();
                    int number = int.Parse(result);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error: I can only accept numbers!", ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.InnerException.StackTrace);
            }
        }

        public async Task<string> HandlingAggregateExceptions()
        {
            try
            {
                //return await FetchData("https://www.google.com"); //uncomment this for something cool :)
                return await FetchData("invalid stuff");
            }
            catch (AggregateException ae)
            {
                ae.Flatten().InnerExceptions.Select(x=>x.Message).ToList().ForEach(Console.WriteLine);
                throw;
            }
        }

        private async Task<string> FetchData(string Uri)
        {
            using HttpClient httpClient = new HttpClient();
            string content = await httpClient.GetStringAsync(Uri);
            return content;
        }
    }
}
