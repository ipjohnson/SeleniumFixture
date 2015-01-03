using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using SeleniumFixture.xUnit.Impl;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public class InternetExplorerDriverAttribute : WebDriverAttribute
    {
        protected override IWebDriver CreateWebDriver(MethodInfo testMethod)
        {
            var chromeDriverProvider = ReflectionHelper.GetAttribute<InternetExplorerDriverProviderAttribute>(testMethod);

            if (chromeDriverProvider != null)
            {
                return chromeDriverProvider.ProvideDriver(testMethod);
            }

            var optionsProvider = ReflectionHelper.GetAttribute<InternetExplorerOptionsAttribute>(testMethod);

            return new InternetExplorerDriver(optionsProvider != null ? 
                                              optionsProvider.ProvideOptions(testMethod) : 
                                              new InternetExplorerOptions());
        }
    }
}
