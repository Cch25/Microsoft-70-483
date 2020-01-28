using System;
using System.Diagnostics;
using System.Threading;

namespace DebugExample
{
    public class PerformanceCounters
    {
        public void PerformanceCounterExample()
        {
            const string CATEGORY_NAME = "Processor Information";
            const string COUNTER_NAME = "% Processor Time";
            const string INSTANCE_NAME = "_Total";

            PerformanceCounter pc = new PerformanceCounter(CATEGORY_NAME, COUNTER_NAME, INSTANCE_NAME);
            Console.WriteLine("Press any key to stop");
            while (true)
            {
                Console.WriteLine($"Processor time {pc.NextValue()}");
                Thread.Sleep(4_00);
                if (Console.KeyAvailable)
                {
                    break;
                }
            }
        }

        public static PerformanceCounter TotalImageCounter;
        public static PerformanceCounter ImagesPerSecondCounter;
        public enum CreationResult
        {
            CreatedCounters,
            LoadedCounters
        };
        private static CreationResult CreatePerformanceCounter()
        {
            string categoryName = "MyCategory";
            if (PerformanceCounterCategory.Exists(categoryName))
            {
                // production code should use using
                TotalImageCounter = new PerformanceCounter(categoryName, "# of images processed", false);
                // production code should use using
                ImagesPerSecondCounter = new PerformanceCounter(categoryName, "# images processed per second", false);
                return CreationResult.LoadedCounters;
            }
            CounterCreationData[] counters = new CounterCreationData[] {
                new CounterCreationData("# of images processed", "number of images resized", PerformanceCounterType.NumberOfItems64),
                new CounterCreationData("# images processed per second","number of images processed per second", PerformanceCounterType.RateOfCountsPerSecond32)
            };
            CounterCreationDataCollection counterCollection = new CounterCreationDataCollection(counters);
            PerformanceCounterCategory.Create(categoryName, "Image processing information", PerformanceCounterCategoryType.SingleInstance, counterCollection);
            return CreationResult.CreatedCounters;

        }
        private void CreateCustomCounter()
        {
            if (CreatePerformanceCounter() == CreationResult.CreatedCounters)
            {
                Console.WriteLine("Performance counters created");
                Console.ReadKey();
            }
        }
        public void RunCustomCounter()
        {
            CreateCustomCounter();
            TotalImageCounter.Increment();
            ImagesPerSecondCounter.Increment();
            //TODO: Fancy algorithm here.
        }
    }
}
