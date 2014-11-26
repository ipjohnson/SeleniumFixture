using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture
{
    public class ImportSeleniumTypePropertySelector : SeleniumTypePropertySelector
    {
        public ImportSeleniumTypePropertySelector(IConstraintHelper helper)
            : base(helper)
        {

        }

        public override IEnumerable<PropertyInfo> SelectProperties(object instance, DataRequest request, ComplexModel model)
        {
            return base.SelectProperties(instance, request, model).Where(p => p.GetCustomAttributes(true).Any(o => o.GetType() == typeof(ImportAttribute))
                                                                           || p.PropertyType == typeof(Fixture)
                                                                           || p.PropertyType == typeof(SimpleFixture.Fixture)
                                                                           || p.PropertyType == typeof(IWebDriver) 
                                                                           || (p.PropertyType == typeof(string) && p.Name.ToLowerInvariant() == "baseaddress"));
        }
    }
}
