using System;
using System.Reflection;
using OpenQA.Selenium.Firefox;

namespace SeleniumFixture.xUnit
{
    public abstract class FirefoxOptionsAttribute : Attribute
    {
        public abstract FirefoxOptions ProvideOptions(MethodInfo testMethod);
    }
}
