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

    public class RemoteDriverAttribute : DataAttribute
    {
        public RemoteDriverAttribute(RemoteWebDriverCapability capability)
        {
            Capability = capability;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var parameters = testMethod.GetParameters().ToArray();

            if (parameters.Length != 1)
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }

            if (parameters[0].ParameterType != typeof(IWebDriver) &&
                parameters[0].ParameterType != typeof(Fixture))
            {
                throw new Exception("Method must take IWebDriver or Fixture");
            }

            foreach (RemoteWebDriverCapability value in Enum.GetValues(typeof(RemoteWebDriverCapability)))
            {
                if ((Capability & value) == value)
                {
                    yield return new object[] { CreateParameter(parameters[0].ParameterType, testMethod, value) };
                }
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

        protected virtual object[] CreateParameter(Type parameterType, MethodInfo testMethod, RemoteWebDriverCapability capability)
        {
            if (parameterType == typeof(IWebDriver))
            {
                return new object[] { CreateWebDriver(testMethod, capability) };
            }

            return new object[] { CreateFixture(testMethod, capability) };
        }

        protected virtual Fixture CreateFixture(MethodInfo testMethod, RemoteWebDriverCapability capability)
        {
            return new Fixture(CreateWebDriver(testMethod, capability));
        }

        protected virtual IWebDriver CreateWebDriver(MethodInfo testMethod, RemoteWebDriverCapability capability)
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

            string hub = Hub;

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

        protected virtual IWebDriver CreateWebDriverInstance(MethodInfo testMethod, string hub, DesiredCapabilities capabilities)
        {
            return string.IsNullOrEmpty(hub) ? new RemoteWebDriver(capabilities) : new RemoteWebDriver(new Uri(hub), capabilities);
        }
    }
}
