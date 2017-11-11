using System;
using System.Reflection;
using OpenQA.Selenium.IE;

namespace SeleniumFixture.xUnit
{
    public abstract class InternetExplorerDriverProviderAttribute : Attribute
    {
        public abstract InternetExplorerDriver ProvideDriver(MethodInfo methodInfo);
    }
}
