using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SimpleFixture.Impl;

namespace SeleniumFixture.Impl
{
    public class PropertySetter : IPropertySetter
    {
        public void SetProperty(PropertyInfo propertyInfo, object instance, object value)
        {
            var methods = propertyInfo.DeclaringType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            var method =
                methods.FirstOrDefault(m => m.Name == "set_" + propertyInfo.Name && m.GetParameters().Count() == 1);

            if (method != null)
            {
                method.Invoke(instance, new[] { value });
            }
        }
    }
}
