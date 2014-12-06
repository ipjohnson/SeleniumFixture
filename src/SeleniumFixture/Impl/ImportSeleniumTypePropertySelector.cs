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
                if(runtimeProperty.GetCustomAttributes(true).All(o => o.GetType() != typeof(ImportAttribute)) && 
                   runtimeProperty.PropertyType != typeof(IActionProvider) && 
                   runtimeProperty.GetSetMethod(true) != null)
                {
                    continue;
                }

                yield return runtimeProperty;
            }
        }
    }
}
