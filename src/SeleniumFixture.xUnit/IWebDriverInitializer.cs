using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    public interface IWebDriverInitializer
    {
        void Initialize(IWebDriver driver);
    }
}
