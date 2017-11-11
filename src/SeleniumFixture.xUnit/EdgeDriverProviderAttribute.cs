using OpenQA.Selenium.Edge;
using System;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public abstract class EdgeDriverProviderAttribute : Attribute
    {
        public abstract EdgeDriver ProvideDriver(MethodInfo testMethod);
    }
}
