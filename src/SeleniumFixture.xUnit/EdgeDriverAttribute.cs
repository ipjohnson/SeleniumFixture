using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using SeleniumFixture.xUnit.Impl;

namespace SeleniumFixture.xUnit
{
    public class EdgeDriverAttribute : WebDriverAttribute
    {
        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            yield return GetOrCreateWebDriver(() => CreateWebDriver(testMethod));
        }

        public override void ReturnDriver(MethodInfo testMethod, IWebDriver driver)
        {
            ReturnDriver(testMethod, driver as EdgeDriver);
        }

        public static EdgeDriver CreateWebDriver(MethodInfo testMethod)
        {
            EdgeDriver driver = null;

            var chromeDriverProvider = ReflectionHelper.GetAttribute<EdgeDriverProviderAttribute>(testMethod);

            if (chromeDriverProvider != null)
            {
                driver = chromeDriverProvider.ProvideDriver(testMethod);
            }
            else
            {
                var optionsProvider = ReflectionHelper.GetAttribute<EdgeOptionsAttribute>(testMethod);

                driver = new EdgeDriver(optionsProvider != null ? optionsProvider.ProvideOptions(testMethod) : new EdgeOptions());
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
