using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumFixture.xUnit.Impl;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SeleniumFixture.xUnit
{
    public class ChromeDriverAttribute : WebDriverAttribute
    {
        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            yield return GetOrCreateWebDriver(testMethod, () => CreateWebDriver(testMethod));
        }
        
        public override void ReturnDriver(MethodInfo testMethod, IWebDriver driver)
        {
            ReturnDriver(testMethod, driver as ChromeDriver);
        }

        private ChromeDriver CreateWebDriver(MethodInfo testMethod)
        {
            ChromeDriver driver = null;

            var chromeDriverProvider = ReflectionHelper.GetAttribute<ChromeDriverProviderAttribute>(testMethod);

            if (chromeDriverProvider != null)
            {
                driver = chromeDriverProvider.ProvideDriver(testMethod);
            }
            else
            {
                var optionsProvider = ReflectionHelper.GetAttribute<ChromeOptionsAttribute>(testMethod);

                var service = ChromeDriverService.CreateDefaultService();
                var options = optionsProvider != null ? optionsProvider.ProvideOptions(testMethod) : new ChromeOptions();
                var commandTimeout = this.GetWebDriverCommandTimeout(testMethod);

                driver = new ChromeDriver(service, options, commandTimeout);
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
