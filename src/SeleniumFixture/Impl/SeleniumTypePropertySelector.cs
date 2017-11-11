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
        public SeleniumTypePropertySelector(IFixtureConfiguration configuration, IConstraintHelper helper) : base(configuration, helper)
        {

        }

        public override IEnumerable<PropertyInfo> SelectProperties(object instance, DataRequest request, ComplexModel model)
        {
            return instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => !p.GetCustomAttributes(true).Any(o => o.GetType().Namespace.StartsWith("OpenQA.Selenium")) &&
                            (p.CanWrite ||
                             p.GetCustomAttributes(typeof(ImportAttribute), true).Any() ||
                             p.PropertyType == typeof(IActionProvider)));
        }
    }
}
