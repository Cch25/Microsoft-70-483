//#define TERSE
#define VERBOSE

using Hierarchies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TypesWithReflection
{
    public class RuntimeReflection
    {
        public void TestConditional()
        {
            ConditionalExampleAttribute cea = new ConditionalExampleAttribute();
            cea.ReportHeader();
            cea.VerboseReport();
            cea.TerseReport();
        }
        public void CheckAttribute()
        {
            CheckSerialization cs = new CheckSerialization();
            cs.SerializationChecker();
        }
        public void CheckCustomAttribute()
        {
            CheckSerialization cs = new CheckSerialization();
            cs.CustomAttributeCheck();
        }
        public void IdentityMembersInClass()
        {
            CheckTypeUsingReflection checkMembers = new CheckTypeUsingReflection();
            checkMembers.IdentifyMembersInClass();
        }
        public void CallMethodUsingReflection()
        {
            CheckTypeUsingReflection checkMembers = new CheckTypeUsingReflection();
            checkMembers.CallingAMethod();
        }
        public void ScanAssembly()
        {
            ScanAssembly scha = new ScanAssembly();
            scha.ScanClassHierarchyAssembly("ClassHierarchy");
            scha.ScanAllAssembliesAvailable();

        }
    }


    #region [ Scan Assembly - That's so f'*** awesome!!! ]
    public class ScanAssembly
    {
        public void ScanClassHierarchyAssembly(string assemblyName)
        {
            Assembly assembly = assemblyName.Contains(".dll") ? 
                               Assembly.LoadFrom(assemblyName): //attention here => LoadFrom and LoadFile
                               Assembly.Load(assemblyName);
            IEnumerable<Type> scanedClasses = assembly.GetTypes()
                .Where(x => typeof(IAccount).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            foreach(Type scanClass in scanedClasses)
            {
                IAccount ac = Activator.CreateInstance(scanClass) as IAccount;
                ac.Withdraw(11);
            }
        }
        public void ScanAllAssembliesAvailable()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach(string dll in Directory.GetFiles(path, "*.dll"))
            {
                ScanClassHierarchyAssembly(dll);
            }
        }
    }
    #endregion

    #region [ Custom Attributes, Scan Members, Invoke Methods ]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProgrammerAttribute : Attribute
    {
        private string programmerValue;
        public ProgrammerAttribute(string programmer)
        {
            programmerValue = programmer;
        }
        public string Programmer
        {
            get { return programmerValue; }
        }

    }

    [Serializable]
    [Programmer("Culai")]
    public class Person<T> where T : class, new()
    {
        [NonSerialized]
        private T adult;
        private int age;
        private T name;

        public Person(T name, int age, T adult)
        {
            this.name = name;
            this.age = age;
            this.adult = adult;
        }
        private void DisplayDetails(T name)
        {
            Console.WriteLine($"My name is {name} and I'm {age} yrs. \nI'm private and hold sensible information.");
        }
        public override string ToString()
        {
            return $"My name is {name} and I'm years {this.age} old.";
        }
    }

    public class CheckTypeUsingReflection
    {
        public void IdentifyMembersInClass()
        {
            Person<object> p = new Person<object>("Culai", 28, "Yes");
            Type type = p.GetType();
            foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.WriteLine(memberInfo.ToString());
            }
        }

        public void CallingAMethod()
        {
            Person<object> p = new Person<object>("Culai", 28, "Yes");
            Type type = p.GetType();
            FieldInfo fieldInfo = type.GetField("age", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(p, 28);
            MethodInfo methodInfo = type.GetMethod("DisplayDetails", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(p, new[] { "Culai" });
        }
    } 
    #endregion

    #region [ Attributes ]
    public class CheckSerialization
    {
        public void SerializationChecker()
        {
            if (Attribute.IsDefined(typeof(Person<>), typeof(SerializableAttribute)))
            {
                Console.WriteLine("Person can be serialized");
            }
            Type classType = typeof(Person<>);
            FieldInfo field = classType.GetField("adult", BindingFlags.NonPublic | BindingFlags.Instance);

            if (Attribute.IsDefined(field, typeof(NonSerializedAttribute)))
            {
                Console.WriteLine($"Private field >>{field.Name}<< is not serializable.");
            }

        }

        public void CustomAttributeCheck()
        {
            Attribute a = Attribute.GetCustomAttribute(typeof(Person<>), typeof(ProgrammerAttribute));
            if (a != null)
            {
                ProgrammerAttribute pa = a as ProgrammerAttribute;
                Console.WriteLine($"Programmer from the {typeof(Person<>).Name} class is {pa.Programmer}");
            }
        }

    }
    public class ConditionalExampleAttribute
    {
        [Conditional("VERBOSE"), Conditional("TERSE")]
        public void ReportHeader()
        {
            Console.WriteLine("This is the header for the report");
        }

        [Conditional("VERBOSE")]
        public void VerboseReport()
        {
            Console.WriteLine("This is the header for the verbose report");
        }

        [Conditional("TERSE")]
        public void TerseReport()
        {
            Console.WriteLine("This is the header for the Terse report");
        }
    } 
    #endregion

}
