using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    public interface IWebDriverFinalizerAttribute
    {
        void Finalize(MethodInfo method, IWebDriver driver);
    }
}
