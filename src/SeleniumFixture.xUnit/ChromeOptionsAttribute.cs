using OpenQA.Selenium.Chrome;
using System;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public abstract class ChromeOptionsAttribute : Attribute
    {
        public abstract ChromeOptions ProvideOptions(MethodInfo testMethod);
    }
}
