using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class InternetExplorerDriverDataDiscoverer : IDataDiscoverer
    {
        public IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            var parameters = testMethod.GetParameters().ToArray();

            if (parameters.Length != 1)
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }

            if (parameters[0].ParameterType.ToRuntimeType() == typeof(IWebDriver))
            {
                yield return new object[] { new InternetExplorerDriver() };
            }
            else if (parameters[0].ParameterType.ToRuntimeType() == typeof(Fixture))
            {
                var driver = new InternetExplorerDriver();

                yield return new object[] { FixtureCreationAttribute.GetNewFixture(driver, testMethod.ToRuntimeMethod()) };
            }
            else
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }
        }

        public bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            return false;
        }

        protected InternetExplorerDriver CreateInternetExplorerDriver(IMethodInfo method)
        {
            InternetExplorerOptionsAttribute attribute =
                ReflectionHelper.GetAttribute<InternetExplorerOptionsAttribute>(method.ToRuntimeMethod());
            InternetExplorerOptions options = null;

            if (attribute != null)
            {
                options = attribute.ProvideProfile();
            }

            if (options == null)
            {
                options = new InternetExplorerOptions();
            }

            return new InternetExplorerDriver(options);
        }
    }
}
