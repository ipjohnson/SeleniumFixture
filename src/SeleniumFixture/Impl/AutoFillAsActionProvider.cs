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
        protected readonly IActionProvider _actionProvider;

        public AutoFillAsActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        public virtual IAutoFillAsAction<T> CreateAction<T>(IEnumerable<IWebElement> elements)
        {
            return new AutoFillAsAction<T>(_actionProvider,elements);
        }
    }
}
