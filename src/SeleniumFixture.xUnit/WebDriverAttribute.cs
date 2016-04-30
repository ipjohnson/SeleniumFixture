using OpenQA.Selenium;
using SeleniumFixture.xUnit.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Creates a new fixture
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        protected virtual Fixture CreateFixture(MethodInfo testMethod)
        {
            var fixture = CreateFixtureInstance(testMethod, CreateWebDriver(testMethod));
            
            var initializer = ProvideFixtureInitializer(testMethod, fixture);

            if (initializer != null)
            {
                initializer.Initialize(fixture);
            }            

            return fixture;
        }

        /// <summary>
        /// Instantiates the fixture instance
        /// </summary>
        /// <param name="testMethod"></param>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        protected virtual Fixture CreateFixtureInstance(MethodInfo testMethod, IWebDriver webDriver )
        {
            return FixtureCreationAttribute.GetNewFixture(webDriver, testMethod);
        }

        /// <summary>
        /// Provides a fixture initializer
        /// </summary>
        /// <param name="testMethod"></param>
        /// <param name="fixture"></param>
        /// <returns></returns>
        protected virtual IFixtureInitializer ProvideFixtureInitializer(MethodInfo testMethod, Fixture fixture)
        {
            var initializeAttribute = ReflectionHelper.GetAttribute<IFixtureInitializationAttribute>(testMethod);

            if (initializeAttribute != null)
            {
                return initializeAttribute.ProvideInitializer(testMethod, fixture);                
            }

            return null;
        }

        protected abstract IWebDriver CreateWebDriver(MethodInfo testMethod);

        protected virtual void InitializeDriver(MethodInfo testMethod, IWebDriver driver)
        {
            var initializer = ProvideWebDriverInitializer(testMethod, driver);

            if(initializer != null)
            {
                initializer.Initialize(driver);
            }
        }

        protected virtual IWebDriverInitializer ProvideWebDriverInitializer(MethodInfo testMethod, IWebDriver driver)
        {
            var initializeAttribute = ReflectionHelper.GetAttribute<IWebDriverInitializationAttribute>(testMethod);

            if (initializeAttribute != null)
            {
                return initializeAttribute.ProvideInitializer(testMethod, driver);
            }

            return null;
        }

    }
}
