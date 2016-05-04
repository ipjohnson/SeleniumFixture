using SeleniumFixture.ExampleModels.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class InitializeToHomePageAttribute : Attribute, IFixtureInitializationAttribute
    {
        public object Initialize(MethodInfo testMethod, Fixture fixture)
        {
            return fixture.Navigate.To<HomePage>("Home.html");
        }
    }
}
