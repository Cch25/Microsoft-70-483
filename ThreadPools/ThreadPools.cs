using System;
using System.Threading;

namespace ThreadPools
{
    public class ThreadPoolsExamples
    {
        private static ManualResetEvent resetEvent = new ManualResetEvent(false);
        
        /// <summary>
        /// Retrieving a thread from pool can be done by using ThreadPool.QueueUserWorkItem(Action<State>)
        /// </summary>
        public void ThreadPoolQueue()
        {
           var tp = ThreadPool.QueueUserWorkItem((state) =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine($"Thread pool proc {i}");
                    Thread.Sleep(1);
                }
                resetEvent.Set();
            });
            resetEvent.WaitOne();
        }
    }
}
