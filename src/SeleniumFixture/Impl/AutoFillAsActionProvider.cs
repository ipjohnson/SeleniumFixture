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
            T seedValue = _actionProvider.UsingFixture.Data.Generate<T>(requestName, constraints);

            return new AutoFillActionProvider(_actionProvider, _elements, seedValue).PerformFill();
        }
    }
}
