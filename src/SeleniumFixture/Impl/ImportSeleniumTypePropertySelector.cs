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
        public ImportSeleniumTypePropertySelector(IConstraintHelper helper)
            : base(helper)
        {

        }

        public override IEnumerable<PropertyInfo> SelectProperties(object instance, DataRequest request, ComplexModel model)
        {
            foreach (PropertyInfo runtimeProperty in instance.GetType().GetRuntimeProperties())
            {
                if (runtimeProperty.GetSetMethod(true) == null)
                {
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
