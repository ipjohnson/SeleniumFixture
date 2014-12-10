using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public class AutoFillAsActionProvider<T>
    {
        private readonly IActionProvider _actionProvider;
        private readonly IEnumerable<IWebElement> _elements;

        public AutoFillAsActionProvider(IActionProvider actionProvider, IEnumerable<IWebElement> elements)
        {
            _actionProvider = actionProvider;
            _elements = elements;
        }
        
        public IThenSubmitActionProvider PerformFill(string requestName, object constraints)
        {
            return new ThenSubmitActionProvider(_actionProvider.UsingFixture, _elements.First());
        }
    }
}
