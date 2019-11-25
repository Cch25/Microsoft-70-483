using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TPLExample
{
    public class ParallelLibrary
    {
        #region [ Helpers ]
        private void TaskOne()
        {
            Console.WriteLine("Task 1 in staring");
            Thread.Sleep(1000);
            Console.WriteLine("Task 1 in ending");
        }
        private void TaskTwo()
        {
            Console.WriteLine("Task 2 in staring");
            Thread.Sleep(1000);
            Console.WriteLine("Task 2 in ending");
        }

        private void WorkingOnItem(int item)
        {
            Console.WriteLine($"Star working on item {item}");
            Thread.Sleep(100);
            Console.WriteLine($"End working with item {item}");
        }
        #endregion

        #region [ Methods ]
        public void ParallelInvoke()
        {
            ParallelOptions po = new ParallelOptions
            {
                MaxDegreeOfParallelism = 5
            };
            Parallel.Invoke(po, () => TaskOne(), () => TaskTwo());
        }

        public void ParallelForeach()
        {
            IEnumerable<int> enumRange = Enumerable.Range(0, 500);
            Parallel.ForEach(enumRange, (item, state) =>
            {
                WorkingOnItem(item);
            });
        }

        public void ParallelFor()
        {
            int[] enumRange = Enumerable.Range(0, 500).ToArray();
            ParallelLoopResult result = Parallel.For(0, enumRange.Length, (item, pls) =>
            {
                if(item == 100)
                {
                    pls.Break();
                    //Break will guarantee that everything under (index) 100 will be completed
                    //Stop will stop at 100, but will not finish all processes 
                }
                WorkingOnItem(item);
            });

        }

        #endregion
    }
}
