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
        protected readonly string Selector;
        protected readonly string JQueryTest;

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

            Selector = selector;
            JQueryTest = jQueryTest;
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

            if (Selector.StartsWith("//"))
            {
                by = XPath(Selector);
            }
            else
            {
                by = IsJavaScriptEnabled(context, JQueryTest) ? Using.JQuery(Selector) : CssSelector(Selector);
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

            var returnValue = executor?.ExecuteScript(jQueryTest);

            if (returnValue is bool b)
            {
                return b;
            }

            return false;
        }
    }
}
