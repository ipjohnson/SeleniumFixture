using System;
using System.Reflection;
using OpenQA.Selenium.Firefox;

namespace SeleniumFixture.xUnit
{
    [Obsolete("FirefoxDriver should not be constructed with a FirefoxProfile object. Use FirefoxOptionsAttribute instead. This attribute will be removed in a future release.")]
    public abstract class FirefoxProfileAttribute : Attribute
    {
        public abstract FirefoxProfile CreateProfile(MethodInfo methodInfo);
    }
}
