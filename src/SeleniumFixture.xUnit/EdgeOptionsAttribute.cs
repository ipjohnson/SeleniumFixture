using OpenQA.Selenium.Edge;
using System;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public abstract class EdgeOptionsAttribute : Attribute
    {
        public abstract EdgeOptions ProvideOptions(MethodInfo method);
    }
}
