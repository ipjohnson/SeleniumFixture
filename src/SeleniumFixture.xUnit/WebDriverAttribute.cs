using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public abstract class WebDriverAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var parameters = testMethod.GetParameters().ToArray();

            if (parameters.Length != 1)
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }

            if (parameters[0].ParameterType == typeof(IWebDriver))
            {
                yield return new object[] { CreateWebDriver(testMethod) };
            }
            else if (parameters[0].ParameterType == typeof(Fixture))
            {
                yield return new object[] { CreateFixture(testMethod) };
            }
            else
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }
        }

        protected virtual Fixture CreateFixture(MethodInfo testMethod)
        {
            return FixtureCreationAttribute.GetNewFixture(CreateWebDriver(testMethod), testMethod);
        }

        protected abstract IWebDriver CreateWebDriver(MethodInfo testMethod);

    }
}
