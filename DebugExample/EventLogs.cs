using System;
using System.Diagnostics;

namespace DebugExample
{
    public enum CreationResult
    {
        CreatedLog,
        LoadedLog
    }
    public class EventLogs
    {
        static EventLog imageEventLog;
        private CreationResult CreateEventLogs()
        {
            string categoryName = "Image Processing";
            if (EventLog.SourceExists(categoryName))
            {
                imageEventLog = new EventLog
                {
                    Source = categoryName
                };
                return CreationResult.LoadedLog;
            }
            EventLog.CreateEventSource(categoryName, categoryName + " log");
            return CreationResult.CreatedLog;
        }
        public void WritingInAnEventLog()
        {
            if(CreateEventLogs() == CreationResult.CreatedLog)
            {
                Console.WriteLine("Log created");
                Console.WriteLine("Restart program");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Processing started");
            imageEventLog.WriteEntry("Image processing started", EventLogEntryType.Warning);
            Console.WriteLine("Processing complete. Press any key to exit.");
            Console.ReadKey();
        }

        public void ReadingFromAnEventLog()
        {
            string categoryName = "Image Processing";
            if (!EventLog.SourceExists(categoryName))
            {
                Console.WriteLine("Event log not present");
            }
            else
            {
                EventLog imageEventLog = new EventLog();
                imageEventLog.Source = categoryName;
                foreach (EventLogEntry entry in imageEventLog.Entries)
                {
                    Console.WriteLine($"Source: {entry.Source} Type: {entry.EntryType} Time: {entry.TimeWritten} Message: {entry.Message}");
                }
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
