using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SeleniumFixture.xUnit.Impl;

namespace SeleniumFixture.xUnit
{
    [Flags]
    public enum RemoteWebDriverCapability
    {
        [Obsolete("Selenium no longer provides an Android device driver.")]
        Andriod = 1,
        [Obsolete("Use the ChromeOptions class to set capabilities for use with Chrome. For use with the Java remote server or grid, use the ToCapabilites method of the ChromeOptions class.")]
        Chrome = 2,
        [Obsolete("Use the FirefoxOptions class to set capabilities for use with Firefox. For use with the Java remote server or grid, use the ToCapabilites method of the FirefoxOptions class.")]
        FireFox = 4,
        HtmlUnit = 8,
        HtmlUnitWithJs = 16,
        [Obsolete("Use the InternetExplorerOptions class to set capabilities for use with Internet Explorer. For use with the Java remote server or grid, use the ToCapabilites method of the InternetExplorerOptions class.")]
        InternetExplorer = 32,
        [Obsolete("Selenium no longer provides an iOS device driver.")]
        Phone = 64,
        [Obsolete("Selenium no longer provides an iOS device driver.")]
        Pad = 128,
        [Obsolete("Use the SafariOptions class to set capabilities for use with Safari. For use with the Java remote server or grid, use the ToCapabilites method of the SafariOptions class.")]
        Safari = 256,
        MajorFour = FireFox | InternetExplorer | Chrome | Safari,

    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RemoteDriverAttribute : WebDriverAttribute
    {
        private RemoteWebDriverCapability _capabilities;

        public RemoteDriverAttribute(RemoteWebDriverCapability capability)
        {
            Capability = capability;
        }

        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            var providerAttribute = ReflectionHelper.GetAttribute<RemoteWebDriverProviderAttribute>(testMethod);

            if (providerAttribute != null)
            {
               yield return providerAttribute.ProvideDriver(testMethod, Capability);
            }
            else
            {
                yield return CreateWebDriver(testMethod, Capability, Hub);
            }
        }

        /// <summary>
        /// Url for grid
        /// </summary>
        public string Hub { get; set; }

        /// <summary>
        /// Capability 
        /// </summary>
        public RemoteWebDriverCapability Capability { get; private set; }

        public static IWebDriver CreateWebDriver(MethodInfo testMethod, RemoteWebDriverCapability capability, string hub)
        {
            DesiredCapabilities capabilities = null;

            switch (capability)
            {
                case RemoteWebDriverCapability.Andriod:
                    capabilities = DesiredCapabilities.Android();
                    break;
                case RemoteWebDriverCapability.Chrome:
                    capabilities = DesiredCapabilities.Chrome();
                    break;
                case RemoteWebDriverCapability.FireFox:
                    capabilities = DesiredCapabilities.Firefox();
                    break;
                case RemoteWebDriverCapability.HtmlUnit:
                    capabilities = DesiredCapabilities.HtmlUnit();
                    break;
                case RemoteWebDriverCapability.HtmlUnitWithJs:
                    capabilities = DesiredCapabilities.HtmlUnitWithJavaScript();
                    break;
                case RemoteWebDriverCapability.InternetExplorer:
                    capabilities = DesiredCapabilities.InternetExplorer();
                    break;
                case RemoteWebDriverCapability.Pad:
                    capabilities = DesiredCapabilities.IPad();
                    break;
                case RemoteWebDriverCapability.Phone:
                    capabilities = DesiredCapabilities.IPhone();
                    break;
                case RemoteWebDriverCapability.Safari:
                    capabilities = DesiredCapabilities.Safari();
                    break;
            }

            if (string.IsNullOrEmpty(hub))
            {
                var attr = ReflectionHelper.GetAttribute<RemoteWebDriverHubAddressAttribute>(testMethod);

                if (attr != null)
                {
                    hub = attr.Hub;
                }
            }

            return CreateWebDriverInstance(testMethod, hub, capabilities);
        }

        public static IWebDriver CreateWebDriverInstance(MethodInfo testMethod, string hub, DesiredCapabilities capabilities)
        {
            var driver = string.IsNullOrEmpty(hub) ? new RemoteWebDriver(capabilities) : new RemoteWebDriver(new Uri(hub), capabilities);

            InitializeDriver(testMethod, driver);

            return driver;
        }

        public override void ReturnDriver(MethodInfo testMethod, IWebDriver driver)
        {
            driver.Dispose();
        }
    }
}
