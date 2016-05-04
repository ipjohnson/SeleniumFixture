using SeleniumFixture.ExampleModels.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class InitializeToInputFormAttribute : Attribute, IFixtureInitializationAttribute
    {
        public object Initialize(MethodInfo testMethod, Fixture fixture)
        {
            fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

            return fixture.Yields<InputPage>();
        }
    }
}
