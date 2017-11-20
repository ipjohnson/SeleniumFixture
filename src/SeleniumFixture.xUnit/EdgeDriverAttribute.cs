using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using SeleniumFixture.xUnit.Impl;

namespace SeleniumFixture.xUnit
{
    public class EdgeDriverAttribute : WebDriverAttribute
    {
        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            yield return GetOrCreateWebDriver(testMethod, () => CreateWebDriver(testMethod));
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

                var service = EdgeDriverService.CreateDefaultService();
                var options = optionsProvider != null ? optionsProvider.ProvideOptions(testMethod) : new EdgeOptions();
                var commandTimeout = GetWebDriverCommandTimeout(testMethod);

                driver = new EdgeDriver(service, options, commandTimeout);
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
