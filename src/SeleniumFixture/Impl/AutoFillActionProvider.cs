using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public class AutoFillActionProvider
    {
        private readonly IActionProvider _actionProvider;
        private readonly IEnumerable<IWebElement> _elements;
        private readonly object _seedWith;

        public AutoFillActionProvider(IActionProvider actionProvider, IEnumerable<IWebElement> elements, object seedWith)
        {
            _actionProvider = actionProvider;
            _elements = elements;
            _seedWith = seedWith;
        }

        public IThenSubmitActionProvider PerformFill()
        {
            return new ThenSubmitActionProvider(_actionProvider.UsingFixture,_elements.First());
        }
    }
}
