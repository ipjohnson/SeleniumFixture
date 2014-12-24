using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public class FireFoxDriverAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var parameters = testMethod.GetParameters();

            if (parameters.Length != 1)
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }

            if (parameters[0].ParameterType == typeof(IWebDriver))
            {
                yield return new object[] { new FirefoxDriver() };
            }
            else if (parameters[0].ParameterType == typeof(Fixture))
            {
                var driver = new FirefoxDriver();

                yield return new object[] { FixtureCreationAttribute.GetNewFixture(driver, testMethod) };
            }
            else
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }
        }
    }
}
