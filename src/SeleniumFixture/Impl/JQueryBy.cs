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
    /// <summary>
    /// Selenium By Selector that uses jQuery
    /// </summary>
    public class JQueryBy : By
    {
        private readonly string _selector;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="selector">jquery selector</param>
        public JQueryBy(string selector)
        {
            _selector = selector;
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
            if (context is IJavaScriptExecutor)
            {
                return FindElementsOnDriver(context as IJavaScriptExecutor);
            }

            if (context is IWrapsDriver)
            {
                return FindElementsOnElement(context as IWrapsDriver);
            }

            throw new Exception("Can't perform JQueryBy in this context: " + context.GetType().FullName);
        }

        private ReadOnlyCollection<IWebElement> FindElementsOnElement(IWrapsDriver wrapsDriver)
        {
            IJavaScriptExecutor executor = wrapsDriver.WrappedDriver as IJavaScriptExecutor;

            if (executor == null)
            {
                throw new Exception("IWrapsDriver does not implement IJavaScriptExecutor");
            }

            string script =
                @"  var returnList = [];
                    $(arguments[0]).find('" + _selector + @"').each(function () { returnList.push(this); });
                    return returnList;";

            var objects = (IEnumerable<object>)executor.ExecuteScript(script, wrapsDriver);

            return new ReadOnlyCollection<IWebElement>(objects.Cast<IWebElement>().ToList());
        }

        private ReadOnlyCollection<IWebElement> FindElementsOnDriver(IJavaScriptExecutor executor)
        {
            IEnumerable<object> matchedItems =
                (IEnumerable<object>)executor.ExecuteScript("return jQuery.find('" + _selector + "')");

            return new ReadOnlyCollection<IWebElement>(matchedItems.Cast<IWebElement>().ToList());
        }
    }
}
