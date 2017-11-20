using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IAutoFillAsActionProvider
    {
        IAutoFillAsAction<T> CreateAction<T>(IEnumerable<IWebElement> elements);
    }

    public class AutoFillAsActionProvider : IAutoFillAsActionProvider
    {
        protected readonly IActionProvider ActionProvider;

        public AutoFillAsActionProvider(IActionProvider actionProvider)
        {
            ActionProvider = actionProvider;
        }

        public virtual IAutoFillAsAction<T> CreateAction<T>(IEnumerable<IWebElement> elements)
        {
            return new AutoFillAsAction<T>(ActionProvider,elements);
        }
    }
}
