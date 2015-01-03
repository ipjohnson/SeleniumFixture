using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumFixture.xUnit.Impl;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public class ChromeDriverAttribute : WebDriverAttribute
    {
        protected override IWebDriver CreateWebDriver(MethodInfo testMethod)
        {
            var chromeDriverProvider = ReflectionHelper.GetAttribute<ChromeDriverProviderAttribute>(testMethod);

            if (chromeDriverProvider != null)
            {
                return chromeDriverProvider.ProvideDriver(testMethod);
            }

            var optionsProvider = ReflectionHelper.GetAttribute<ChromeOptionsAttribute>(testMethod);

            return new ChromeDriver(optionsProvider != null ? optionsProvider.ProvideOptions(testMethod) : new ChromeOptions());
        }
    }
}
