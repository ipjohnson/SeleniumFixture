using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture.Impl
{
    public class ImportSeleniumTypePropertySelector : SeleniumTypePropertySelector
    {
        public ImportSeleniumTypePropertySelector(IFixtureConfiguration configuration, IConstraintHelper helper)
            : base(configuration, helper)
        {

        }

        public override IEnumerable<PropertyInfo> SelectProperties(object instance, DataRequest request, ComplexModel model)
        {
            foreach (PropertyInfo runtimeProperty in instance.GetType().GetRuntimeProperties())
            {
                if (runtimeProperty.GetSetMethod(true) == null)
                {
                    if (runtimeProperty.DeclaringType == null ||
                        runtimeProperty.DeclaringType == instance.GetType())
                        continue;

                    var baseProperty = runtimeProperty.DeclaringType.GetProperty(runtimeProperty.Name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if(baseProperty == null || baseProperty.GetSetMethod(true) == null)
                        continue;
                }

                if(runtimeProperty.GetCustomAttributes(true).Any(o => o.GetType() == typeof(ImportAttribute)) ||
                   runtimeProperty.PropertyType == typeof(IActionProvider))
                {
                    yield return runtimeProperty;
                }
            }
        }
    }
}
