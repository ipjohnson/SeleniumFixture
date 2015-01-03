using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SeleniumFixture.xUnit.Impl;
using Xunit.Abstractions;

namespace SeleniumFixture.xUnit
{
    public abstract class FixtureCreationAttribute : Attribute
    {
        public abstract Fixture CreateFixture(IWebDriver driver);

        public static Fixture GetNewFixture(IWebDriver driver, MethodInfo method)
        {
            FixtureCreationAttribute creationAttribute = ReflectionHelper.GetAttribute<FixtureCreationAttribute>(method);

            if (creationAttribute != null)
            {
                return creationAttribute.CreateFixture(driver);
            }

            string baseAddress = ConfigurationManager.AppSettings["BaseAddress"];

            return new Fixture(driver,baseAddress);
        }
    }
}
