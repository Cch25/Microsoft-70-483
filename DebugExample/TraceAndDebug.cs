using System;
using System.Diagnostics;

namespace DebugExample
{
    public class TraceAndDebug
    {
        /// <summary>
        /// The Debug will work only if the Solution Configurations is set to Debug.
        /// If you need to run this in production, then you should use Trace.
        /// </summary>
        public void DebugMethod()
        {
            Debug.WriteLine("Starting the program");
            Debug.Indent();
            Debug.WriteLine("Inside a function");
            Debug.Unindent();
            Debug.WriteLine("Outside a function");
            string customer = "CC";
            Debug.WriteLineIf(!string.IsNullOrEmpty(customer), "The name is NOT empty");
        }
        public void TraceMethod()
        {
            Trace.WriteLine("Starting the program");
            Trace.WriteLine("Good weather");
            Trace.WriteLineIf(true, "Good times");
            Trace.TraceWarning("Something is warningy");
        }

        /// <summary>
        /// If the assetion fails, the program will not continue
        /// </summary>
        public void DebugAssertions()
        {
            string name = "CC";
            Debug.Assert(string.IsNullOrEmpty(name), "The name is empty");
            Debug.Assert(!string.IsNullOrEmpty(name), "The name is not empty");
        }
        /// <summary>
        /// ConsoleTraceListener Sends the output to the console;
        /// DelimitedTextTraceListener Sends the output to a TextWriter;
        /// EventLogTraceListener Sends the output to the Event log;   
        /// EventSchemaTraceListener Sends the output to an XML encoded file compliant with the Event log schema;
        /// TextWriterTraceListener Sends the output to a given TextWriter;
        /// XMLWriterTraceListener Sends XML formatted output to an XML writer;
        /// </summary>
        public void TraceListeners()
        {
            TraceListener traceListener = new ConsoleTraceListener();
            Trace.Listeners.Add(traceListener);
            //Trace.Listeners.Remove(traceListener);
            Trace.TraceInformation("Info test");
            Trace.TraceWarning("Warning test");
            Trace.TraceError("Error test");
        }

        public void TraceSources()
        {
            TraceListener traceListener = new ConsoleTraceListener();
            TraceSource traceSource = new TraceSource("configControl", SourceLevels.All);
            SourceSwitch sourceSwitch = new SourceSwitch("Switch", "Control for tracing");
            sourceSwitch.Level = SourceLevels.Error;
            traceSource.Switch = sourceSwitch;
            traceSource.TraceEvent(TraceEventType.Verbose, 10000, "Trace verbose");
            traceSource.TraceEvent(TraceEventType.Information, 10001, "Trace info");
            traceSource.TraceEvent(TraceEventType.Warning, 10002, "Trace warning");
            traceSource.TraceEvent(TraceEventType.Error, 10003, "Trace error");
            traceSource.TraceEvent(TraceEventType.Critical, 10004, "Trace critical");
            traceSource.TraceData(TraceEventType.Information, 1003, new object[] { "Note 1", "Message 2" });
            traceSource.Listeners.Add(traceListener);
            traceSource.Flush();
            traceSource.Close();
        }
    }
}
