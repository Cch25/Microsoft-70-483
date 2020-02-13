using Synchronization;
using Tasks;
using ThreadPools;
using Threads;
using TPLExample;
using ParallelCollectionExample;
using EventHandlers;
using ExceptionsExample;
using Types;
using ConsumeType;
using Hierarchies;
using TypesWithReflection;
using StringManipulation;
using Encryption;
using DebugExample;
using Files;
using ConsumeData;

namespace _70_483_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            ChapterOne();
            ChapterTwo();
            ChapterThree();
            ChapterFour();
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
            #region [ Create types ]
            CreateTypes createTypes = new CreateTypes();
            //createTypes.StructAndClasses();
            //createTypes.Aliens();
            //createTypes.MyStackGeneric();
            //createTypes.ThisConstructor();
            //createTypes.DestoryAlien();
            //createTypes.SummaryOfCh1to2_1(); 
            #endregion

            #region [ Consume types ]
            ConsumeTypes consumeTypes = new ConsumeTypes();
            //consumeTypes.BoxingAndUnboxing();
            //consumeTypes.ImplicitExplicitOperator();
            //consumeTypes.UsingDynamic(); 
            #endregion

            #region [ Class hierarchies ]
            ClassHierarchy ch = new ClassHierarchy();
            //ch.DepositInMyBank(20);
            //ch.ComparePricesInMyBank();
            #endregion

            #region [ Reflection ]

            #region [ Attributes, Methods Invoke, Load Assemblies]
            RuntimeReflection reflection = new RuntimeReflection();
            //reflection.TestConditional();
            //reflection.CheckAttribute();
            //reflection.CheckCustomAttribute();
            //reflection.IdentityMembersInClass();
            //reflection.CallMethodUsingReflection();
            //reflection.ScanAssembly(); 
            #endregion

            #region [ Code DOM , Expression Trees and Reflection ]

            #region [ CodeDOM ]
            CodeDOMExample cde = new CodeDOMExample();
            //cde.GenerateCodDOM();

            #endregion

            #region [ Expression Trees ]
            ExpressionTrees et = new ExpressionTrees();
            //et.ExpressionTreeMultiply();
            //et.ExpressionTreeModifyToAdd();
            //et.IsAdultFemale(); 
            #endregion

            #region [ More Reflection ]
            Assemblies assemblies = new Assemblies();
            //assemblies.DisplayAssemblyInfo();
            //assemblies.GetPropertyInfo();
            //assemblies.GetMethodInfo(); 
            #endregion

            #endregion

            #endregion

            #region [ Manipulate strings ]
            ManipulateStrings manipulateStrings = new ManipulateStrings();
            //manipulateStrings.StringInterning();
            //manipulateStrings.StringWriter();
            //manipulateStrings.StringReader();
            //manipulateStrings.SearchStrings();
            //manipulateStrings.StringComparisonAndCulture();
            //manipulateStrings.FormatString();
            //manipulateStrings.FormattableString();
            //manipulateStrings.MusicTrackFormatter();
            #endregion

        }
        #endregion

        #region [ Chapter Three ]
        public static void ChapterThree()
        {
            #region [ Symmetric and asymmetric encryption]
            SymmetricAndAsymmetricEncryption saae = new SymmetricAndAsymmetricEncryption();

            #endregion

            #region [ Trace and debug ]
            TraceAndDebug tad = new TraceAndDebug();
            //tad.DebugMethod();
            //tad.TraceMethod();
            //tad.DebugAssertions();
            //tad.TraceListeners();
            //tad.TraceSources();
            PerformanceCounters pc = new PerformanceCounters();
            //pc.PerformanceCounterExample();
            //pc.RunCustomCounter();
            EventLogs eventLogs = new EventLogs();
            //eventLogs.WritingInAnEventLog();
            //eventLogs.ReadingFromAnEventLog();
            //eventLogs.EventLogBinder();
            #endregion
        }
        #endregion

        #region [ ChapterFour ]
        public static void ChapterFour()
        {
            #region [ File I/O and Network]
            FilesExample fe = new FilesExample();
            //fe.FileStream();
            //fe.FileStreamImprove();
            //fe.StreamWriterAndReader();
            //fe.ChainStreams();
            //fe.FileHelperClass();
            //fe.ExceptionHandling();
            //fe.FileStorage();
            //fe.FileInfo();
            //fe.Directory();
            //fe.SearchFiles();
            NetworkReadsAndWrites nraw = new NetworkReadsAndWrites();
            //_ = nraw.WebRequest();
            //_ = nraw.WebClient();
            //_ = nraw.HttpClient();
            #endregion

            #region [ Consume Data ]
            ConsumeDataExample cd = new ConsumeDataExample();
            //cd.ConsumeJsonData();
            //cd.ConsumeXmlData();
            //cd.ConsumeXmlDataInDOM();
            #endregion

            #region [ LINQ ]

            #endregion
        }
        #endregion
    }
}
