using System;
using System.Reflection;

namespace TypesWithReflection
{
    public class Assemblies
    {
        private readonly AssemblyInfos _assemblyInfos = new AssemblyInfos();
        public void DisplayAssemblyInfo()
        {
            _assemblyInfos.GetAssemblyDetails();
        }
        public void GetPropertyInfo()
        {
            _assemblyInfos.PropertyInfo();
        }

        public void GetMethodInfo()
        {
            _assemblyInfos.MethodInfo();
        }


    }
    public class AssemblyInfos
    {
        public void GetAssemblyDetails()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Full name {assembly.FullName}");

            AssemblyName assemblyName = assembly.GetName();
            Console.WriteLine($"Assembly name {assemblyName}");

            Console.WriteLine($"Major version {assemblyName.Version.Major}");
            Console.WriteLine($"Minor version {assemblyName.Version.Minor}");

            Console.WriteLine($"In GAC {assembly.GlobalAssemblyCache}");

            foreach (Module assemblyModule in assembly.Modules)
            {
                Console.WriteLine($"_Module {assemblyModule.Name}");
                foreach (Type module in assemblyModule.GetTypes())
                {
                    Console.WriteLine($"\t__Type {module.Name}");
                    foreach (MemberInfo member in module.GetMembers())
                    {
                        Console.WriteLine($"\t\t___Member: {member.Name}");
                    }
                }
            }
        }

        public void PropertyInfo()
        {
            Type type = typeof(ReflectionPerson);
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    Console.WriteLine($"Can Read. Set method {propertyInfo.GetMethod}");
                }
                if (propertyInfo.CanWrite)
                {
                    Console.WriteLine($"Can Write. Set method {propertyInfo.SetMethod}");
                }
            }
        }

        public void MethodInfo()
        {
            Type type = typeof(ReflectionPerson);
            MethodInfo method = type.GetMethod("AddInt", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodBody methodBody = method.GetMethodBody();
            foreach (byte b in methodBody.GetILAsByteArray())
            {
                Console.Write($"{b:X} ");
            }

            ReflectionPerson rp = new ReflectionPerson();
            object[] inputs = new object[] { 5, 5 };
            int result = (int)method.Invoke(rp, inputs);
            Console.WriteLine($"\nResult {result}");

            result = (int)type.InvokeMember("AddInt",
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                null, rp, inputs);
            Console.WriteLine($"Result {result}");
        }
    }

    public class ReflectionPerson
    {
        public string Name { get; set; }
        public int Age { get; }

        private int AddInt(int v1, int v2)
        {
            return v1 + v2;
        }
    }
}
