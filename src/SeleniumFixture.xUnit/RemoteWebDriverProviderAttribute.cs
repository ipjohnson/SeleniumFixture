using System;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium;

namespace SeleniumFixture.xUnit
{
    public abstract class RemoteWebDriverProviderAttribute : Attribute
    {
        public abstract IEnumerable<IWebDriver> ProvideDriver(MethodInfo testMethod, RemoteWebDriverCapability capability);
    }
}
