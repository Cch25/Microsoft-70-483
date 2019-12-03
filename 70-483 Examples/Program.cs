using Synchronization;
using Tasks;
using ThreadPools;
using Threads;
using TPLExample;
using ParallelCollectionExample;
using EventHandlers;
using ExceptionsExample;
using Types;

namespace _70_483_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            ChapterOne();
            ChapterTwo();
           
        }


        #region [ Chapter One ]
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

            #region [ Exceptions and Custom Exceptions]

            #region [ Exceptions ]
            Exceptions exceptions = new Exceptions();
            //exceptions.ReadNumbersFromUser();
            //exceptions.ReadNumberDivideByZeroOrWrongFormat();
            //PropagateInnerExeption(exceptions);
            //exceptions.CatchExceptionFromAnotherMethod(); 
            //exceptions.HandlingInnerExceptions();
            //System.Console.WriteLine(exceptions.HandlingAggregateExceptions().GetAwaiter().GetResult());
            #endregion

            #region [ Custom Exceptions ]
            CustomExceptions customExceptions = new CustomExceptions();
            //customExceptions.MyCustomException();
            #endregion

            #endregion
        }

        #region [ Exception Helpers ] 
        public static void PropagateInnerExeption(Exceptions exceptions)
        {
            try
            {
                exceptions.ThrowYourOwnExceptionWithInnerException();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex.InnerException.Message);
            }
        }
        #endregion

        #endregion

        #region [ Chapter Two ]
        public static void ChapterTwo()
        {
            CreateTypes createTypes = new CreateTypes();
            //createTypes.StructAndClasses();
            //createTypes.Aliens();
            //createTypes.MyStackGeneric();
            //createTypes.ThisConstructor();
            //createTypes.DestoryAlien();
        }
        #endregion
    }
}
