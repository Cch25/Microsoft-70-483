using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
    public class SynchronizationPracticalExample
    {
        private static long _sharedTotal;
        private static readonly object syncLock = new object();
        private static readonly int[] items = Enumerable.Range(0, 500_000_001).ToArray();
        private void AddRangeOfValues(int start, int end)
        {
            #region [ Bad approach ]
            //while (start < end)
            //{
            //    lock (syncLock)
            //    {
            //        _sharedTotal += items[start];
            //    }
            //    start++;
            //} 
            #endregion

            long subTotal = 0;
            while (start < end)
            {
                subTotal += items[start];
                start++;
            }

            #region [ Using lock is great, but we can also use Interlocked]
            //lock (syncLock)
            //{
            //    _sharedTotal += subTotal;
            //} 
            #endregion

            Interlocked.Add(ref _sharedTotal, subTotal);


        }
        public void ComputeSumOfLargeArrayOneThread()
        {
            Stopwatch startWatch = Stopwatch.StartNew();
            startWatch.Start();
            long total = 0;
            for (int i = 0; i < items.Length; i++)
                total = total + items[i];
            startWatch.Stop();
            Console.WriteLine($"Total time is {startWatch.ElapsedMilliseconds}");
            Console.WriteLine("The total is: {0}", total);
        }
        public void ComputeSumOfLargeArrayMultiThreaded()
        {
            Stopwatch startWatch = Stopwatch.StartNew();
            startWatch.Start();
            List<Task> tasks = new List<Task>();
            int rangeSize = 1_000;
            int rangeStart = 0;
            while (rangeStart < items.Length)
            {
                int rangeEnd = rangeStart + rangeSize;
                if (rangeEnd > items.Length)
                {
                    rangeEnd = items.Length;
                }
                // create local copies of the parameters
                int rs = rangeStart;
                int re = rangeEnd;

                tasks.Add(Task.Run(() => AddRangeOfValues(rs, re)));
                rangeStart = rangeEnd;
            }
            Task.WaitAll(tasks.ToArray());
            startWatch.Stop();
            Console.WriteLine($"Total time is {startWatch.ElapsedMilliseconds}");

            Console.WriteLine($"Total computed sum is :{_sharedTotal}");
        }


    }
}
