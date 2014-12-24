using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    public class ReflectionHelper
    {
        public static T GetAttribute<T>(MethodInfo methodInfo) where T : Attribute
        {
            T returnAttribute = methodInfo.GetCustomAttribute(typeof(T), true) as T ??
                                (methodInfo.DeclaringType.GetCustomAttribute(typeof(T), true) as T ??
                                 methodInfo.DeclaringType.Assembly.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T);

            return returnAttribute;
        }
    }
}
