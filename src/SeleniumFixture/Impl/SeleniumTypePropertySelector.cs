using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SeleniumFixture.Impl;
using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture
{
    public class SeleniumTypePropertySelector : TypePropertySelector
    {
        public SeleniumTypePropertySelector(IConstraintHelper helper) : base(helper)
        {

        }

        public override IEnumerable<PropertyInfo> SelectProperties(object instance, DataRequest request, ComplexModel model)
        {
            return instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.CanWrite ||
                            p.GetCustomAttributes(typeof(ImportAttribute), true).Any() ||
                            p.PropertyType == typeof(IActionProvider));

            // Skip populating properties that have selenium attributes
            return base.SelectProperties(instance, request, model).
                        Where(p => !p.GetCustomAttributes(true).Any(o => o.GetType().Namespace.StartsWith("OpenQA.Selenium")));
        }
    }
}
