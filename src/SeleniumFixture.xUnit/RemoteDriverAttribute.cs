using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using SeleniumFixture.xUnit.Impl;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    [Flags]
    public enum RemoteWebDriverCapability
    {
        Andriod = 1,
        Chrome = 2,
        FireFox = 4,
        HtmlUnit = 8,
        HtmlUnitWithJS = 16,
        InternetExplorer = 32,
        IPhone = 64,
        IPad = 128,
        Safari = 256,
        MajorFour = FireFox | InternetExplorer | Chrome | Safari,

    }

    public class RemoteDriverAttribute : WebDriverAttribute
    {
        public RemoteDriverAttribute(RemoteWebDriverCapability capability)
        {
            Capability = capability;
        }       

        public override IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod)
        {
            yield return CreateWebDriver(testMethod, Capability, Hub);
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
                case RemoteWebDriverCapability.HtmlUnitWithJS:
                    capabilities = DesiredCapabilities.HtmlUnitWithJavaScript();
                    break;
                case RemoteWebDriverCapability.InternetExplorer:
                    capabilities = DesiredCapabilities.InternetExplorer();
                    break;
                case RemoteWebDriverCapability.IPad:
                    capabilities = DesiredCapabilities.IPad();
                    break;
                case RemoteWebDriverCapability.IPhone:
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
