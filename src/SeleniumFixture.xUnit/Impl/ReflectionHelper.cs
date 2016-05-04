using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            returnList.AddRange(methodInfo.GetCustomAttributes().Where(a => a is T).Select(a => a as T));

            if(returnList.Count == 0)
            {
                returnList.AddRange(methodInfo.DeclaringType.GetCustomAttributes().Where(a => a is T).Select(a => a as T));
            }

            if(returnList.Count == 0)
            {
                returnList.AddRange(methodInfo.DeclaringType.Assembly.GetCustomAttributes().Where(a => a is T).Select(a => a as T));
            }

            return returnList;
        }
    }
}
