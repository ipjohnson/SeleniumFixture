using System;
using System.Reflection;
using OpenQA.Selenium;
using SeleniumFixture.xUnit.Impl;

namespace SeleniumFixture.xUnit
{
    public abstract class FixtureCreationAttribute : Attribute
    {
        public abstract Fixture CreateFixture(IWebDriver driver);

        public static Fixture GetNewFixture(IWebDriver driver, MethodInfo method)
        {
            var creationAttribute = ReflectionHelper.GetAttribute<FixtureCreationAttribute>(method);

            if (creationAttribute != null)
            {
                return creationAttribute.CreateFixture(driver);
            }
            
            return new Fixture(driver);
        }
    }
}
