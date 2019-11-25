using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelCollectionExample
{
    public class BlockingCollectionsExamples
    {
        /// <summary>
        /// Blocking Collection is a wrapper around other concurrent collection
        /// such as: Concurrent Stack, Cuncurrent Queue or Concurrent Dictionary
        /// </summary>
        public void BlockingCollections()
        {
            using BlockingCollection<int> data = new BlockingCollection<int>(5);
            Task.Run(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine($"Adding new item {i}");
                    data.Add(i);
                }
                data.CompleteAdding();
            }).ConfigureAwait(false);

            Task.Run(() =>
            {
                try
                {
                    while (!data.IsCompleted)
                    {
                        Console.WriteLine($"Getting out {data.Take()}");
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                }
            }).Wait();

        }

        /// <summary>
        /// A BlockingCollection is indeed a wrapper over the rest of the what you know
        /// stack, queue, dictionary, but a consumer gets the value imediately, i does not wait to be blocked
        /// by lets say "until there are 5 there" it imediatelly get the value and consumes it, if there are more than
        /// 5 unconsumed then it stops. Wrapper means that you can decide the order on which you can Take them out
        /// </summary>
        public void BlockingCollectionConcurrentQueue()
        {
            using BlockingCollection<int> blockingCollection = new BlockingCollection<int>(new ConcurrentQueue<int>(), 5);
            Task.Run(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    blockingCollection.Add(i);
                    Console.WriteLine($"Adding {i}");

                }
                blockingCollection.CompleteAdding();
            });

            Task.Run(() =>
            {
                try
                {
                    while (!blockingCollection.IsCompleted)
                    {
                        int res = blockingCollection.Take();
                        Thread.Sleep(100);
                        Console.WriteLine($"Removing - {res}");
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                }

            }).Wait();

        }
    }
}
