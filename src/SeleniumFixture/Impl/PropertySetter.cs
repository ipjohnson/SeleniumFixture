using System.Linq;
using System.Reflection;
using SimpleFixture.Impl;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// SimpleFixture property setter, allows the user to inject properties into private members
    /// </summary>
    public class PropertySetter : IPropertySetter
    {
        public void SetProperty(PropertyInfo propertyInfo, object instance, object value)
        {
            var methods = propertyInfo.DeclaringType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            var method =
                methods.FirstOrDefault(m => m.Name == "set_" + propertyInfo.Name && m.GetParameters().Length == 1);

            if (method != null)
            {
                method.Invoke(instance, new[] { value });
            }
        }
    }
}
