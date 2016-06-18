using System;
using System.Reflection;

namespace MEAMDataGen
{
    static class ConsoleUtils
    {
        public static T Read<T>(string prompt, T defaultValue)
        {
            var parseMethod = typeof(T).GetMethod("Parse", BindingFlags.Static);
            if (parseMethod == null && typeof(T) != typeof(string))
                Console.WriteLine("Error! Can't parse!");

            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            if (typeof(T) == typeof(string))
                return (T)(object)input;

            return (T)parseMethod.Invoke(null, new object[] { input });
        }
    }
}
