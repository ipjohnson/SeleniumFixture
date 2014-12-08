using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace SeleniumFixture.Impl
{
    public class AutoBy : By
    {
        private readonly string _selector;
        private readonly string _jQueryTest;

        public AutoBy(string selector, string jQueryTest = "return window.jQuery;")
        {
            if (string.IsNullOrEmpty(selector))
            {
                throw new ArgumentNullException("selector", "Must provide a string to select by");
            }

            if (string.IsNullOrEmpty(jQueryTest))
            {
                throw new ArgumentNullException("jQueryTest", "Must provide a valid javascript statement");
            }

            _selector = selector;
            _jQueryTest = jQueryTest;
        }

        public override IWebElement FindElement(ISearchContext context)
        {
            return FindElements(context).First();
        }

        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            By by = null;

            if (_selector.StartsWith("//"))
            {
                by = XPath(_selector);
            }
            else
            {
                by = IsJavaScriptEnabled(context) ? Using.JQuery(_selector) : CssSelector(_selector);
            }

            return context.FindElements(by);
        }

        private bool IsJavaScriptEnabled(ISearchContext context)
        {
            IJavaScriptExecutor executor = context as IJavaScriptExecutor;

            if (executor == null && context is IWrapsDriver)
            {
                executor = ((IWrapsDriver)context).WrappedDriver as IJavaScriptExecutor;
            }

            if (executor != null)
            {
                object returnValue = executor.ExecuteScript(_jQueryTest);

                if (returnValue is bool)
                {
                    return (bool)returnValue;
                }
            }

            return false;
        }
    }
}
