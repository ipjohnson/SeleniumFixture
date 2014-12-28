using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit;

namespace SeleniumFixture.xUnit.Tests
{
    public class FireFoxDriverAttributeTests
    {
        [SeleniumTheory, FireFoxDriver, InternetExplorerDriver, ChromeDriver]
        public void FireFoxDriverAttribute_DriverParameter_ExecuteCorrectly(Fixture fixture)
        {
            fixture.Navigate.To("http://www.google.com");
        }
    }
}
