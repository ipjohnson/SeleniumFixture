using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
