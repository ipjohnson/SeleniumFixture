using System;
using System.Linq;
using System.Reflection;

namespace SeleniumFixture.xUnit.Impl
{
    public class ReflectionHelper
    {
        public static T GetAttribute<T>(MethodInfo methodInfo) where T : Attribute
        {
            Attribute returnAttribute = methodInfo.GetCustomAttributes().FirstOrDefault(a => a is T) ??
                                (methodInfo.DeclaringType.GetCustomAttributes().FirstOrDefault(a => a is T) ??
                                 methodInfo.DeclaringType.Assembly.GetCustomAttributes().FirstOrDefault(a => a is T));

            return returnAttribute as T;
        }
    }
}
