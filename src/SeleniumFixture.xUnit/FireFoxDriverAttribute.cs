using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using SeleniumFixture.xUnit.Impl;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public class FirefoxDriverAttribute : WebDriverAttribute
    {
        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            yield return GetOrCreateWebDriver(testMethod, () => CreateWebDriver(testMethod));
        }

        public override void ReturnDriver(MethodInfo testMethod, IWebDriver driver)
        {
            ReturnDriver(testMethod, driver as FirefoxDriver);
        }

        public static FirefoxDriver CreateWebDriver(MethodInfo testMethod)
        {
            FirefoxDriver driver = null;

            var firefoxDriverProvider = ReflectionHelper.GetAttribute<FirefoxDriverProviderAttribute>(testMethod);

            if (firefoxDriverProvider != null)
            {
                driver = firefoxDriverProvider.ProvideDriver(testMethod);
            }
            else
            {
                var provider = ReflectionHelper.GetAttribute<FirefoxProfileAttribute>(testMethod);


                driver = new FirefoxDriver(provider != null ?
                                         provider.CreateProfile(testMethod) :
                                         new FirefoxProfile());
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
