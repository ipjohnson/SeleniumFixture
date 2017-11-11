using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace SeleniumFixture.xUnit
{
    public abstract class RemoteWebDriverProviderAttribute : Attribute
    {
        public abstract IEnumerable<IWebDriver> ProvideDriver(IMethodInfo testMethod, RemoteWebDriverCapability capability);
    }
}
