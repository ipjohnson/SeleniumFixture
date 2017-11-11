using OpenQA.Selenium;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public interface IWebDriverFinalizerAttribute
    {
        void Finalize(MethodInfo method, IWebDriver driver);
    }
}
