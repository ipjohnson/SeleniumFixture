using OpenQA.Selenium;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public interface IWebDriverInitializationAttribute
    {
        void Initialize(MethodInfo method, IWebDriver driver);
    }
}
