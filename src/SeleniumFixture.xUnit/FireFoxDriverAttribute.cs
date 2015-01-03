using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using SeleniumFixture.xUnit.Impl;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public class FirefoxDriverAttribute : WebDriverAttribute
    {
        protected override IWebDriver CreateWebDriver(MethodInfo testMethod)
        {
            var firefoxDriverProvider = ReflectionHelper.GetAttribute<FirefoxDriverProviderAttribute>(testMethod);

            if (firefoxDriverProvider != null)
            {
                return firefoxDriverProvider.ProvideDriver(testMethod);
            }

            var provider = ReflectionHelper.GetAttribute<FirefoxProfileAttribute>(testMethod);


            return new FirefoxDriver(provider != null ? 
                                     provider.CreateProfile(testMethod) : 
                                     new FirefoxProfile());
        }
    }
}
