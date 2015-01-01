using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Xunit;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    [DataDiscoverer("SeleniumFixture.xUnit.Impl.FirefoxDriverDataDiscoverer", "SeleniumFixture.xUnit")]
    public class FirefoxDriverAttribute : DataAttribute
    {
        private Guid _guid = Guid.NewGuid();

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield break;
        }
    }
}
