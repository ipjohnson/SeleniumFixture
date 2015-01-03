using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public abstract class FirefoxDriverProviderAttribute : Attribute
    {
        public abstract FirefoxDriver ProvideDriver(MethodInfo methodInfo);
    }
}
