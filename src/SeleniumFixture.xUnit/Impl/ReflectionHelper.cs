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
    }
}
