using Synchronization;
using Tasks;
using ThreadPools;
using Threads;
using TPLExample;
using ParallelCollectionExample;
using EventHandlers;

namespace _70_483_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            ChapterOne();
        }

        private static void ChapterOne()
        {
            #region [ MultiThreading ]

            #region [ Threads ]
            ThreadsExamples threads = new ThreadsExamples();
            //threads.RunThreads();
            //threads.RunThreadsInBackground(true);
            //threads.ParameterizedThread(5);
            //threads.StopAThread();
            //threads.ThreadStatic();
            //threads.UseThreadLocalVariable(); 
            #endregion

            #region [ ThreadPools ]
            ThreadPoolsExamples threadPools = new ThreadPoolsExamples();
            //threadPools.ThreadPoolQueue(); 
            #endregion

            #region [ Tasks ]
            TasksExample tasksExample = new TasksExample();
            //tasksExample.TaskRunAndWait();
            //tasksExample.GenericTaskRunAndWait();
            //tasksExample.ContinuationTask();
            //tasksExample.ContinuationTaskOptions();
            //tasksExample.AttachToParentTask();
            //tasksExample.AttachToParentTaskWithFactory();
            //tasksExample.WaitAnyTask(); 
            #endregion

            #region [ Thread Synchronization ]
            SynchronizationExample sync = new SynchronizationExample();
            //sync.IgnoreSynchronization();
            //sync.SyncWithLock();
            //sync.SyncDeadLock();
            //sync.AtomicOperationWithInterlock();
            //sync.InterlockExchange();
            //sync.CancelLongRunningMethods();
            //sync.ThrowCancellationException();
            //sync.CancelTasksWithTaskAny();
            //sync.CancellationTokenPracticalExample(); 
            #endregion

            #region [ Thread sync practical example]
            SynchronizationPracticalExample spe = new SynchronizationPracticalExample();
            //spe.ComputeSumOfLargeArrayOneThread();
            //spe.ComputeSumOfLargeArrayMultiThreaded(); 
            #endregion

            #region [ Parallel library ]
            ParallelLibrary tpl = new ParallelLibrary();
            //tpl.ParallelInvoke();
            //tpl.ParallelForeach();
            //tpl.ParallelFor(); 
            #endregion

            #region [ Concurrent Collections ]
            BlockingCollectionsExamples bc = new BlockingCollectionsExamples();
            //bc.BlockingCollections();
            //bc.BlockingCollectionConcurrentQueue(); 
            #endregion

            #endregion

            #region [ Delegates, Events, Callbacks and Lambdas]
         
            #region [ Events ]
            EventHandlerExample eventHandler = new EventHandlerExample();
            //eventHandler.RaiseEvent();
            //eventHandler.RaiseEventSecure();
            //eventHandler.RaiseEventHandler();
            //eventHandler.RaiseEventHandlerWithExeptionHandling(); 
            #endregion

            #region [ Delegates ]
            DelegatesAndLambdas dla = new DelegatesAndLambdas();
            //dla.CreateDelegate(); 
            #endregion

            #region [ Lambdas ]
            //dla.Lambda();
            //dla.Closure();
            #endregion  

            #endregion



        }
    }
}
