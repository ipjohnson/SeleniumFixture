using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SeleniumFixture.xUnit.Impl;

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
                var profileProvider = ReflectionHelper.GetAttribute<FirefoxProfileAttribute>(testMethod);
                if (profileProvider != null)
                {
                    // depreicated provider
                    var binary = new FirefoxBinary();
                    var profile = profileProvider.CreateProfile(testMethod);
                    var commandTimeout = GetWebDriverCommandTimeout(testMethod);

                    driver = new FirefoxDriver(binary, profile, commandTimeout);
                }
                else
                {
                    var optionsProvider = ReflectionHelper.GetAttribute<FirefoxOptionsAttribute>(testMethod);

                    var service = FirefoxDriverService.CreateDefaultService();
                    var options = optionsProvider != null ? optionsProvider.ProvideOptions(testMethod) : new FirefoxOptions();
                    var commandTimeout = GetWebDriverCommandTimeout(testMethod);

                    driver = new FirefoxDriver(service, options, commandTimeout);
                }
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
