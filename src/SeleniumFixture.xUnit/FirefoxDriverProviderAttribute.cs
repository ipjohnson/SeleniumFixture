using System;
using System.Reflection;
using OpenQA.Selenium.Firefox;

namespace SeleniumFixture.xUnit
{
    public abstract class FirefoxDriverProviderAttribute : Attribute
    {
        public abstract FirefoxDriver ProvideDriver(MethodInfo methodInfo);
    }
}
