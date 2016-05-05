using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumFixture.xUnit.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public static ChromeDriver CreateWebDriver(MethodInfo testMethod)
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

                driver = new ChromeDriver(optionsProvider != null ? optionsProvider.ProvideOptions(testMethod) : new ChromeOptions());
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
