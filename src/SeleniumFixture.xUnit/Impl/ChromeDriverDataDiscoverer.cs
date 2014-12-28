using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class ChromeDriverDataDiscoverer : IDataDiscoverer
    {
        public virtual IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            var parameters = testMethod.GetParameters().ToArray();

            if (parameters.Length != 1)
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }

            if (parameters[0].ParameterType.ToRuntimeType() == typeof(IWebDriver))
            {
                yield return new object[] { CreateChromeDriver(testMethod) };
            }
            else if (parameters[0].ParameterType.ToRuntimeType() == typeof(Fixture))
            {
                var driver = CreateChromeDriver(testMethod);
                
                yield return new object[] { FixtureCreationAttribute.GetNewFixture(driver, testMethod.ToRuntimeMethod()) };
            }
            else
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }
        }

        public virtual bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            return false;
        }

        protected virtual ChromeDriver CreateChromeDriver(IMethodInfo methodInfo)
        {
            ChromeOptionsAttribute attribute =
                ReflectionHelper.GetAttribute<ChromeOptionsAttribute>(methodInfo.ToRuntimeMethod());
            ChromeOptions options = null;

            if (attribute != null)
            {
                options = attribute.ProvideOptions();
            }

            if (options == null)
            {
                options = new ChromeOptions();
            }

            return new ChromeDriver(options);
        }
    }
}
