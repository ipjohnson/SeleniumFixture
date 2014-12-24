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
        [Theory,FireFoxDriver]
        public void FireFoxDriverAttribute_DriverParameter_ExecuteCorrectly(IWebDriver driver)
        {

            
        }
    }
}
