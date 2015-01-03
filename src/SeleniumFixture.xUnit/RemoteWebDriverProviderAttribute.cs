using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace SeleniumFixture.xUnit
{
    public abstract class RemoteWebDriverProviderAttribute : Attribute
    {
        public abstract IEnumerable<IWebDriver> ProvideDriver(IMethodInfo testMethod, RemoteWebDriverCapability capability);
    }
}
