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
                var optionsProvider = ReflectionHelper.GetAttribute<FirefoxOptionsAttribute>(testMethod);
                var provider = ReflectionHelper.GetAttribute<FirefoxProfileAttribute>(testMethod);

                if (optionsProvider == null && provider == null)
                {
                    driver = new FirefoxDriver();
                }
                else if (optionsProvider != null)
                {
                    var options = optionsProvider.ProvideOptions(testMethod);
                    var service = FirefoxDriverService.CreateDefaultService();
                    driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(60));
                }
                else
                {
                    var profile = provider.CreateProfile(testMethod);
                    driver = new FirefoxDriver(profile);
                }
            }

            InitializeDriver(testMethod, driver);

            return driver;
        }
    }
}
