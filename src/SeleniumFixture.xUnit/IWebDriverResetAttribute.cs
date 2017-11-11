using OpenQA.Selenium;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public interface IWebDriverResetAttribute
    {
        void ResetWebDriver(MethodInfo testMethod, IWebDriver driver);
    }
}
