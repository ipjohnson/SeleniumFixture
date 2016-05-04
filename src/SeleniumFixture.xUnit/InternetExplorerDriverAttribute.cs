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

        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            yield return GetOrCreateWebDriver(() => CreateWebDriver(testMethod));
        }
        
        public override void ReturnDriver(MethodInfo testMethod, IWebDriver driver)
        {
            ReturnDriver(testMethod, driver as InternetExplorerDriver);
        }

        public static InternetExplorerDriver CreateWebDriver(MethodInfo testMethod)
        {
            InternetExplorerDriver driver = null;

            var chromeDriverProvider = ReflectionHelper.GetAttribute<InternetExplorerDriverProviderAttribute>(testMethod);

            if (chromeDriverProvider != null)
            {
                driver = chromeDriverProvider.ProvideDriver(testMethod);
            }
            else
            {
                var optionsProvider = ReflectionHelper.GetAttribute<InternetExplorerOptionsAttribute>(testMethod);

                driver = new InternetExplorerDriver(optionsProvider != null ?
                                                  optionsProvider.ProvideOptions(testMethod) :
                                                  new InternetExplorerOptions {  IgnoreZoomLevel = true });
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }

    }
}
