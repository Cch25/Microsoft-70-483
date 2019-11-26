using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventHandlers
{
    public class EventHandlerExample
    {
        public void RaiseEvent()
        {
            Alarm alarm = new Alarm();
            alarm.OnAlarmRaised += (date) => { Console.WriteLine($"Raised sub 1 at {date}"); };
            alarm.OnAlarmRaised += (date) => { Console.WriteLine($"Raised sub 2 at {date}"); };
            alarm.RaiseAlarm();
        }
        public void RaiseEventSecure()
        {
            AlarmSecure alarmSecure = new AlarmSecure();
            alarmSecure.OnAlarmRaised += (date) => Console.WriteLine($"Alarm is secured {date}");
            alarmSecure.RaiseAlarm();
        }
        public void RaiseEventHandler()
        {
            AlarmEventHandler aeh = new AlarmEventHandler();
            aeh.OnAlarmRaised += (sender, evt) => { Console.WriteLine($"Wake up {evt.Name} you're {evt.Age} :)"); };
            aeh.OnAlarmRaised += (sender, evt) =>
            {
                evt.Name = "Culy";
                evt.Age = 29;
                Console.WriteLine($"Wake up {evt.Name} you're {evt.Age} next year :)");
            };
            aeh.OnAlarmRaised += (sender, evt) => { Console.WriteLine($"Wake up {evt.Name} you're {evt.Age} :)"); };
            aeh.RaiseAlarm();
        }

        public void RaiseEventHandlerWithExeptionHandling()
        {
            AlarmEventHandlerWithExeptionHandling aehweh = new AlarmEventHandlerWithExeptionHandling();
            aehweh.OnAlarmRaised += (sender, evt) =>
            {
                Console.WriteLine($"Wake up {evt.Name} you're {evt.Age} :)");
                throw new Exception("bang");
            };
            aehweh.OnAlarmRaised += (sender, evt) =>
            {
                evt.Name = "Culy";
                evt.Age = 29;
                Console.WriteLine($"Wake up {evt.Name} you're {evt.Age} next year :)");
                throw new Exception("bum");
            };
            aehweh.OnAlarmRaised += (sender, evt) => { Console.WriteLine($"Wake up {evt.Name} you're {evt.Age} :)"); };
            try
            {
                aehweh.RaiseAlarm();
            }
            catch (AggregateException ae)
            {
                ae.Flatten().InnerExceptions.Select(x => x.Message).ToList().ForEach(Console.WriteLine);
            }
        }

    }


    #region [ Helpers ]
    /// <summary>
    /// Create a basic Action delegate, and call invoking it in a method...
    /// but wait, there's more below :)
    /// </summary>
    public class Alarm
    {
        public Action<DateTime> OnAlarmRaised { get; set; }

        public void RaiseAlarm()
        {
            OnAlarmRaised?.Invoke(DateTime.Now);
        }
    }

    /// <summary>
    /// We can use the event keyword on the property to signal that that's an event
    /// and we can also initialize it right from the bat, 
    /// to avoid checking for null when invoking it
    /// </summary>
    public class AlarmSecure
    {
        public event Action<DateTime> OnAlarmRaised = (date) => { };
        public void RaiseAlarm()
        {
            OnAlarmRaised(DateTime.Now);
        }
    }

    /// <summary>
    /// If we want to signal or pass data, we can make use of EventHandler from .NET 
    /// this will allow us to pass an object (current preferably) and arguments
    /// While this is fun and cool, something's very wrong here...If a subscriber decides to 
    /// change a property from the event args...well... the next subscribe will have to suffer,
    /// because it will get the changed value from what that subscriber did. Your choice.
    /// </summary>
    public class AlarmEventArgs : EventArgs
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
    public class AlarmEventHandler
    {
        public event EventHandler<AlarmEventArgs> OnAlarmRaised = (s, e) => { };
        public void RaiseAlarm()
        {
            OnAlarmRaised(this, new AlarmEventArgs { Age = 28, Name = "Culai" });
        }
    }

    /// <summary>
    /// But what if one or more subscribers throws an unhandled exception?
    /// Then what? We can make use of this awesome GetInvocationList() :)
    /// This will allow us to dynamically invoke a delegate foreach subscriber
    /// If it fails, then we can add it into a list, that will hold our awesome Exeptions
    /// we can later throw them as an AggregateExeption error !!!AWESOME!!!
    /// ALSO, if a subscriber changes values, it doesn't matter at all.
    /// </summary>
    public class AlarmEventHandlerWithExeptionHandling
    {
        public event EventHandler<AlarmEventArgs> OnAlarmRaised = (s, e) => { };
        public void RaiseAlarm()
        {
            List<Exception> errors = new List<Exception>();
            foreach (Delegate handler in OnAlarmRaised.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, new AlarmEventArgs { Age = 28, Name = "Culai" });
                }
                catch (TargetInvocationException tie)
                {
                    errors.Add(tie.InnerException);
                }
            }
            if (errors.Count > 0)
            {
                throw new AggregateException(errors);
            }
        }
    }

    #endregion
}
