using System;
using System.Threading;

namespace Threads
{
    public class ThreadsExamples
    {
        #region [ Helpers ] 
        private static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Thread proc {i}");
                Thread.Sleep(0);
            }
        }
        private static void ThreadBackgroundMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Thread proc {i}");
                Thread.Sleep(1000);
            }
        }

        private static void PassParameterToThread(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine($"Thread proc {i}");
                Thread.Sleep(0);
            }
        }
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Run a bunch of threads just to see how they work
        /// use thread.Join() to wait them to finish executing
        /// </summary>
        public void RunThreads()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.Start();
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Main thread do some work");
                Thread.Sleep(0);
            }
            t.Join();
        }

        /// <summary>
        /// Run the operation in background as long as the foreground operation is not
        /// terminated, for example you can listen for a socket change, while you're doing
        /// something else
        /// </summary>
        /// <param name="isBackground">Set true to make it run in the background</param>
        public void RunThreadsInBackground(bool isBackground)
        {
            new Thread(new ThreadStart(ThreadBackgroundMethod))
            {
                IsBackground = isBackground,
                Priority = ThreadPriority.Lowest,
            }.Start();
        }

        /// <summary>
        /// You can pass a parameter (object) to a thread
        /// using the ParameterizedThreadStart instance
        /// </summary>
        /// <param name="o">in our case this will be an integer value.</param>
        public void ParameterizedThread(object o)
        {
            new Thread(new ParameterizedThreadStart(PassParameterToThread))
            {
                Name = "Test",
                IsBackground = false,
                Priority = ThreadPriority.Normal
            }.Start(o);
        }

        /// <summary>
        /// Stopping a thread can be made using Thread.Abort a thing that should be avoid
        /// because you don't know what thread is stopping it, or when.
        /// What we should do is to Create a variable that can be shared across threads
        /// </summary>
        public void StopAThread()
        {
            bool stopped = false;
            Thread t = new Thread(new ThreadStart(() =>
             {
                 while (!stopped)
                 {
                     Console.WriteLine("Running...");
                     Thread.Sleep(100);
                 }
             }));
            t.Start();

            Console.ReadKey();
            stopped = true;
            t.Join();
        }

        [ThreadStatic]
        private static int _field;
        /// <summary>
        /// If you have a variable that you need to get a copy for each thread,
        /// then you should use ThreadStatic attribute, without this attribute the same variable
        /// will get incremented by the threads. (This type of variable is not local)
        /// </summary>
        public void ThreadStatic()
        {
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _field++;
                    Console.WriteLine($"Thread A: {_field}");
                }
            }).Start();
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _field++;
                    Console.WriteLine($"Thread B: {_field}");
                }
            }).Start();
        }

        public static ThreadLocal<int> _threadLocalField = new ThreadLocal<int>(() => 5);
        /// <summary>
        /// If you want to use a thread local variable then you can make use of ThreadLocal<T> field
        /// </summary>
        public void UseThreadLocalVariable()
        {
            new Thread(() =>
            {
                for (int i = 0; i < _threadLocalField.Value; i++)
                {
                    Console.WriteLine($"Thread A proc: {i}");
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < _threadLocalField.Value; i++)
                {
                    Console.WriteLine($"Thread B proc: {i}");
                }
            }).Start();

        }




        #endregion
    }
}
