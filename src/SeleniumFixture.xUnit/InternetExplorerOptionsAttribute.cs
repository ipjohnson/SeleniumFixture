using System;
using System.Reflection;
using OpenQA.Selenium.IE;

namespace SeleniumFixture.xUnit
{
    public abstract class InternetExplorerOptionsAttribute : Attribute
    {
        public abstract InternetExplorerOptions ProvideOptions(MethodInfo methodInfo);
    }
}
