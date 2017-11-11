using OpenQA.Selenium.Chrome;
using System;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public abstract class ChromeDriverProviderAttribute : Attribute
    {
        public abstract ChromeDriver ProvideDriver(MethodInfo testMethod);
    }
}
