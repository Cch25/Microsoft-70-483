using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
    public class SynchronizationExample
    {
        /// <summary>
        /// Ignoring synchronization mean that there might be threads that access
        /// the same resource at the same time, causing unexpected results.
        /// This behaviour is caused by the two threads working at the same time
        /// taking the value let's assume 5, and one is incrementing it, and the same one
        /// decrements it.
        /// </summary>
        public void IgnoreSynchronization()
        {
            int n = 0;
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 1_000_000; i++)
                {
                    n++;
                }
            });
            for (int i = 0; i < 1_000_000; i++)
            {
                n--;
            }
            t.Wait();
            Console.WriteLine(n);
        }

        /// <summary>
        /// We can easily use the Monitor class that is described as a syntactic sugar as 
        /// "lock", where we can block the current thread that's entering over and disallow
        /// everyone trying to enter.
        /// </summary>
        public void SyncWithLock()
        {
            int n = 0;
            object syncLock = new object();
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 1_000_000; i++)
                {
                    lock (syncLock)
                    {
                        n++;
                    }
                }
            });
            for (int i = 0; i < 1_000_000; i++)
            {
                lock (syncLock)
                {
                    n--;
                }
            }
            t.Wait();
            Console.WriteLine(n);
        }


        /// <summary>
        /// Using lock is good, but you have to understand how a deadlock can occur
        /// if you lock A then you lock B, and you are inside B and try to Call A the 
        /// threads blocks
        /// </summary>
        public void SyncDeadLock()
        {
            object lockA = new object();
            object lockB = new object();

            Task.Run(() =>
            {
                lock (lockA)
                {
                    Console.WriteLine("Locking on A");
                    Console.WriteLine("Waiting on B");
                    lock (lockB)
                    {
                        Console.WriteLine("A and B are locked");
                    }
                }
            });

            Task.Run(() =>
            {
                lock (lockB)
                {
                    Console.WriteLine("Locking on B");
                    Console.WriteLine("Waiting on A");
                    lock (lockA)
                    {
                        Console.WriteLine("B and A are locked");
                    }
                }
            }).Wait();
        }

        /// <summary>
        /// Use atomic operation on threads using Interlock
        /// </summary>
        public void AtomicOperationWithInterlock()
        {
            int n = 0;
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 1_000_000; i++)
                {
                    Interlocked.Increment(ref n);
                }
            });

            for (int i = 0; i < 1_000_000; i++)
            {
                Interlocked.Decrement(ref n);
            }

            t.Wait();
            Console.WriteLine(n);

        }

        /// <summary>
        /// InterlockExchange allows you to compare current value and change its value
        /// based on a condition
        /// </summary>
        static int value = 1;
        public void InterlockExchange()
        {
            Task t = Task.Run(() =>
            {
                Interlocked.CompareExchange(ref value, 2, 0);
                //this will NOT set the value to two, since comparand is 0
            });
            Task t2 = Task.Run(() =>
             {
                 Interlocked.CompareExchange(ref value, 3, 2);
                 //this will NOT set the vlaue to 3, since comparand is not 2
             });
            Task.WaitAll(t, t2);
            Console.WriteLine(value);
        }

        /// <summary>
        /// We can make use of the CancellationTokenSource and pass its Token property
        /// as a CancellationToken
        /// </summary>
        public void CancelLongRunningMethods()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var t = Task.Run(() =>
            {
                cts.CancelAfter(3_000);
                for (int i = 0; i < 1_00; i++)
                {
                    Thread.Sleep(100);
                    Console.Write("*");
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancelation was initiated");
                        cts.Cancel();
                        break;
                    }
                }
            }, token);
            t.Wait();
        }

        /// <summary>
        /// You can also throw an exception if you want to 
        /// </summary>
        public void ThrowCancellationException()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task t = Task.Run(() =>
               {
                   int n = 0;
                   for (int i = 0; i < 1_00; i++)
                   {
                       Thread.Sleep(50);
                       Console.Write("*");
                       n++;
                       if (n > 80)
                       {
                           cts.Cancel();
                           Console.WriteLine();
                       }
                       token.ThrowIfCancellationRequested();
                   }
               });
            try
            {
                t.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Flatten().InnerExceptions.Select(x => x.Message).ToList().ForEach(Console.WriteLine);
            }
        }

        /// <summary>
        /// We can also use a second parameter to Task.WaitAny(_,time) which will cancel
        /// a long running operation
        /// </summary>
        public void CancelTasksWithTaskAny()
        {
            Task t = Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    Console.Write("*");
                }
            });

            int index = Task.WaitAny(new[] { t }, 5_000);
            if (index < 0)
            {
                Console.WriteLine("\nSorry, your tasks timed out");
            }
        }

        /// <summary>
        /// We can carefully study this example to understand the basics of CTS
        /// </summary>
        /// 
        private static void Clock(CancellationToken token)
        {
            int tickCount = 0;
            while (!token.IsCancellationRequested && tickCount < 20)
            {
                tickCount++;
                Console.WriteLine("Tick");
                Thread.Sleep(500);
            }
            token.ThrowIfCancellationRequested();
        }
        public void CancellationTokenPracticalExample()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task clock = Task.Run(() => Clock(token));

            Console.WriteLine("Press any key to stop the clock");
            Console.ReadKey();
            if (clock.IsCompleted)
            {
                Console.WriteLine("Clock task completed");
            }
            else
            {
                try
                {
                    cts.Cancel();
                    clock.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Flatten().InnerExceptions.ToList().Select(x => x.Message).ToList().ForEach(Console.WriteLine);
                }
            }

        }
    }
}
