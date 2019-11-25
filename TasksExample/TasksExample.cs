using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class TasksExample
    {
        /// <summary>
        /// You can make a task run with Task.Run factory method
        /// then if you want that that to be executed you neet to wait for it
        /// using the Wait() method
        /// </summary>
        public void TaskRunAndWait()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.Write("*");
                    Thread.Sleep(10);
                }
            }).Wait();

        }

        /// <summary>
        /// Task can also return a T value 
        /// </summary>
        public void GenericTaskRunAndWait()
        {
            Task<int>.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.Write("*");
                }
                return 5;
            }).Wait();
        }

        /// <summary>
        /// A task can also support continuation after a specific task is finished
        /// </summary>
        public void ContinuationTask()
        {
            Task<int> result = Task.Run(() =>
            {
                return 41;
            }).ContinueWith(x =>
            {
                return x.Result * x.Result;
            });
            Console.WriteLine(result.Result);
        }

        /// <summary>
        /// You can also add continuation options on different errors or even when it's finished
        /// don't forget when you ContinueWith, last one needs to be awaited
        /// </summary>
        public void ContinuationTaskOptions()
        {
            Task<int> task = Task.Run(() =>
            {
                return 41;
            });

            task.ContinueWith(val =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task completed = task.ContinueWith(val =>
           {
               Console.WriteLine($"Ran to completion with value {val.Result}");
           }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completed.Wait();
        }

        /// <summary>
        /// After creating a task you can create new Task(()=>) and use AttachToParent creationoptions
        /// this will happen inside the parent Task.Run
        /// </summary>
        public void AttachToParentTask()
        {

            Task<int[]> parent = Task<int>.Run(() =>
            {
                int[] vals = new int[3];
                new Task(() =>
                {
                    vals[0] = 5;
                }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() =>
                {
                    vals[2] = 1;

                }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() =>
                {
                    vals[1] = 2;
                }, TaskCreationOptions.AttachedToParent).Start();
                return vals;
            });
            Task finalTask = parent.ContinueWith(data =>
            {
                foreach (var item in data.Result)
                {
                    Console.WriteLine(item);
                }
            });
            finalTask.Wait();
        }

        /// <summary>
        /// Instead of calling attachtoparent every time you create a subtask, you can create a factory with properties
        /// then pass your logic in there (it's just a shorthand)
        /// </summary>
        public void AttachToParentTaskWithFactory()
        {
            Task<int[]> parent = Task.Run(() =>
            {
                int[] result = new int[3];

                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);

                tf.StartNew(() =>
                {
                    result[1] = 2;
                });
                tf.StartNew(() =>
                {
                    result[0] = 5;
                });
                tf.StartNew(() =>
                {
                    result[2] = 3;
                });

                return result;
            });

            foreach(var p in parent.Result)
            {
                Console.WriteLine(p);
            }
        }

        /// <summary>
        /// You can also wait for a task, all or any
        /// </summary>
        public void WaitAnyTask()
        {
            Task<int>[] tasks = new Task<int>[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 1;
            });
            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 2;
            });
            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 3;
            });

            while (tasks.Length > 0)
            {
                int index = Task.WaitAny(tasks);
                Console.WriteLine($"Task {index} is finished and returned {tasks[index].Result}");
                List<Task<int>> temp = tasks.ToList();
                temp.RemoveAt(index);
                tasks = temp.ToArray();
            }

        }
    }
}
