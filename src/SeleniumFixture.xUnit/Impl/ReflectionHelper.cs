using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SeleniumFixture.xUnit.Impl
{
    public static class ReflectionHelper
    {
        public static T GetAttribute<T>(MethodInfo methodInfo) where T : class
        {
            Attribute returnAttribute = methodInfo.GetCustomAttributes().FirstOrDefault(a => a is T) ??
                                (methodInfo.DeclaringType.GetCustomAttributes().FirstOrDefault(a => a is T) ??
                                 methodInfo.DeclaringType.Assembly.GetCustomAttributes().FirstOrDefault(a => a is T));

            return returnAttribute as T;
        }

        public static IEnumerable<T> GetAttributes<T>(MethodInfo methodInfo) where T : class
        {
            List<T> returnList = new List<T>();

            returnList.AddRange(methodInfo.GetCustomAttributes().OfType<T>());

            if(returnList.Count == 0)
            {
                returnList.AddRange(methodInfo.DeclaringType.GetCustomAttributes().OfType<T>());
            }

            if(returnList.Count == 0)
            {
                returnList.AddRange(methodInfo.DeclaringType.Assembly.GetCustomAttributes().OfType<T>());
            }

            return returnList;
        }
    }
}
