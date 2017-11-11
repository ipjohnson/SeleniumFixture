using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Selenium By selector that uses JQuery, CSS Selector, or XPath depending on the selector and if jquery is avaliable
    /// </summary>
    public class AutoBy : By
    {
        protected readonly string _selector;
        protected readonly string _jQueryTest;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="jQueryTest"></param>
        public AutoBy(string selector, string jQueryTest = "return typeof jQuery != 'undefined';")
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

        /// <summary>
        /// Finds the first element matching the criteria.
        /// </summary>
        /// <param name="context">An <see cref="T:OpenQA.Selenium.ISearchContext"/> object to use to search for the elements.</param>
        /// <returns>
        /// The first matching <see cref="T:OpenQA.Selenium.IWebElement"/> on the current context.
        /// </returns>
        public override IWebElement FindElement(ISearchContext context)
        {
            return FindElements(context).First();
        }

        /// <summary>
        /// Finds all elements matching the criteria.
        /// </summary>
        /// <param name="context">An <see cref="T:OpenQA.Selenium.ISearchContext"/> object to use to search for the elements.</param>
        /// <returns>
        /// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
        ///             matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            By by = null;

            if (_selector.StartsWith("//"))
            {
                by = XPath(_selector);
            }
            else
            {
                by = IsJavaScriptEnabled(context, _jQueryTest) ? Using.JQuery(_selector) : CssSelector(_selector);
            }

            return context.FindElements(by);
        }

        public static bool IsJavaScriptEnabled(ISearchContext context, string jQueryTest = "return typeof jQuery != 'undefined';")
        {
            var executor = context as IJavaScriptExecutor;

            if (executor == null && context is IWrapsDriver)
            {
                executor = ((IWrapsDriver)context).WrappedDriver as IJavaScriptExecutor;
            }

            if (executor != null)
            {
                var returnValue = executor.ExecuteScript(jQueryTest);

                if (returnValue is bool)
                {
                    return (bool)returnValue;
                }
            }

            return false;
        }
    }
}
