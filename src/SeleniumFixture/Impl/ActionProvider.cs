using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using SimpleFixture;

namespace SeleniumFixture.Impl
{
    public enum ClickMode
    {
        /// <summary>
        /// Click all elements returned, throws exception when there are none
        /// </summary>
        ClickAll,

        /// <summary>
        /// Click any element returned, does not throw an exception if no elements are found
        /// </summary>
        ClickAny,

        /// <summary>
        /// Click one and only one element, throws exception when there isn't exactly one element
        /// </summary>
        ClickOne,

        /// <summary>
        /// Click the first element, throws exception when there are no element
        /// </summary>
        ClickFirst,
    }

    public interface IActionProvider
    {
        /// <summary>
        /// Navigate to a specified url, base address will be added to any relative URL
        /// </summary>
        /// <param name="url">url to navigate to</param>
        /// <returns>this</returns>
        IActionProvider NavigateTo(string url = null);

        /// <summary>
        /// Navigate and Yield a Page Object model
        /// </summary>
        /// <typeparam name="T">Page Object type</typeparam>
        /// <param name="url">url to navigate to</param>
        /// <returns>page object</returns>
        T NavigateTo<T>(string url = null);

        /// <summary>
        /// Find a specified element by selector
        /// </summary>
        /// <param name="selector">selector to use to locate element</param>
        /// <returns>element or throws an exception</returns>
        IWebElement Find(string selector);

        /// <summary>
        /// Find a specified by selector
        /// </summary>
        /// <param name="selector">by selector</param>
        /// <returns>elements</returns>
        IWebElement Find(By selector);

        /// <summary>
        /// Find All elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        ReadOnlyCollection<IWebElement> FindAll(string selector);

        /// <summary>
        /// Find all elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        ReadOnlyCollection<IWebElement> FindAll(By selector);

        /// <summary>
        /// Check for the element specified in the selector
        /// </summary>
        /// <param name="selector">selector to look for</param>
        /// <returns>true if element exists</returns>
        bool CheckForElement(string selector);

        /// <summary>
        /// Check for the element specified in the selector
        /// </summary>
        /// <param name="selector">selector to look for</param>
        /// <returns>true if element exists</returns>
        bool CheckForElement(By selector);

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default </param>
        /// <returns>this</returns>
        IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll);

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default </param>
        /// <returns>this</returns>
        IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll);

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>this</returns>
        IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll);

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>this</returns>
        IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll);

        /// <summary>
        /// Drag an element
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>this</returns>
        IDragActionProvider Drag(string selector);
        
        /// <summary>
        /// Drag an element
        /// </summary>
        /// <param name="selector">element</param>
        /// <returns>this</returns>
        IDragActionProvider Drag(By selector);

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFill(string selector, object seedWith = null);

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFill(By selector, object seedWith = null);

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="seedWith"></param>
        /// <returns></returns>
        IThenSubmitActionProvider AutoFill(IEnumerable<IWebElement> elements, object seedWith = null);

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        IFillActionProvider Fill(string selector);

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        IFillActionProvider Fill(By selector);
        
        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns>fill action</returns>
        IFillActionProvider Fill(IEnumerable<IWebElement> elements);

        /// <summary>
        /// Wait for something to happen
        /// </summary>
        IWaitActionProvider Wait { get; }

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        IYieldsActionProvider Submit(string selector);

        /// <summary>
        /// Fixture for this action provider
        /// </summary>
        Fixture UsingFixture { get; }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <typeparam name="T">Type of object to Generate</typeparam>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new T</returns>
        T Yields<T>(string requestName = null, object constraints = null);

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <param name="type">Type of object to Generate</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new instance</returns>
        object Yields(Type type, string requestName = null, object constraints = null);
    }

    /// <summary>
    /// Action provider
    /// </summary>
    public class ActionProvider : IActionProvider
    {
        protected readonly Fixture _fixture;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture"></param>
        public ActionProvider(Fixture fixture)
        {
            _fixture = fixture;
        }

        public IActionProvider NavigateTo(string url = null)
        {
            if (url == null || !url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                url = _fixture.Configuration.BaseAddress + url;
            }

            _fixture.Driver.Navigate().GoToUrl(url);

            return this;
        }

        public T NavigateTo<T>(string url = null)
        {
            NavigateTo(url);

            return _fixture.Data.Locate<T>();
        }

        public IWebElement Find(string element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            switch (_fixture.Configuration.Selector)
            {
                case SelectorAlgorithm.JQuery:
                    return FindByJQuery(element);

                case SelectorAlgorithm.CSS:
                    return _fixture.Driver.FindElement(By.CssSelector(element));

                case SelectorAlgorithm.XPath:
                    return _fixture.Driver.FindElement(By.XPath(element));

                case SelectorAlgorithm.Auto:
                    return FindByAuto(element);

                default:
                    throw new Exception("Unknown SelectorAlgorithm " + _fixture.Configuration.Selector);

            }
        }

        public IWebElement Find(By selector)
        {
            return _fixture.Driver.FindElement(selector);
        }

        public ReadOnlyCollection<IWebElement> FindAll(string selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            switch (_fixture.Configuration.Selector)
            {
                case SelectorAlgorithm.JQuery:
                    return FindAllByJQuery(selector, true);

                case SelectorAlgorithm.CSS:
                    return _fixture.Driver.FindElements(By.CssSelector(selector));

                case SelectorAlgorithm.XPath:
                    return _fixture.Driver.FindElements(By.XPath(selector));

                case SelectorAlgorithm.Auto:
                    return FindAllByAuto(selector);

                default:
                    throw new Exception("Unknown SelectorAlgorithm " + _fixture.Configuration.Selector);

            }
        }

        public ReadOnlyCollection<IWebElement> FindAll(By element)
        {
            return _fixture.Driver.FindElements(element);
        }

        public bool CheckForElement(string element)
        {
            return FindAll(element).Any();
        }

        public bool CheckForElement(By element)
        {
            return FindAll(element).Any();
        }

        public IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    Find(selector).Click();
                    break;
                case ClickMode.ClickAny:
                    FindAll(selector).Apply(c => c.Click());
                    break;
                case ClickMode.ClickAll:
                    var all = FindAll(selector);

                    if (all.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    all.Apply(c => c.Click());
                    break;
                case ClickMode.ClickFirst:
                    var firstList = FindAll(selector);

                    if (firstList.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    firstList[0].Click();
                    break;
            }

            return this;
        }

        public IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    Find(selector).Click();
                    break;
                case ClickMode.ClickAny:
                    FindAll(selector).Apply(c => c.Click());
                    break;
                case ClickMode.ClickAll:
                    var all = FindAll(selector);

                    if (all.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    all.Apply(c => c.Click());
                    break;
                case ClickMode.ClickFirst:
                    var firstList = FindAll(selector);

                    if (firstList.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    firstList[0].Click();
                    break;
            }

            return this;
        }

        public IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            throw new NotImplementedException();
        }

        public IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            throw new NotImplementedException();
        }
        
        public IDragActionProvider Drag(string element)
        {
            throw new NotImplementedException();
        }

        public IDragActionProvider Drag(By element)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFill(string element, object seedWith = null)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFill(By selector, object seedWith = null)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFill(IEnumerable<IWebElement> elements, object seedWith = null)
        {
            throw new NotImplementedException();
        }

        public IFillActionProvider Fill(string selector)
        {
            return Fill(FindAll(selector));
        }

        public IFillActionProvider Fill(By selector)
        {
            return Fill(FindAll(selector));
        }
        
        public IFillActionProvider Fill(IEnumerable<IWebElement> elements)
        {
            return new FillActionProvider(elements, _fixture);
        }

        public IWaitActionProvider Wait
        {
            get { return new WaitActionProvider(this); }
        }

        public IYieldsActionProvider Submit(string element)
        {
            Find(element).Submit();

            return new YieldsActionProvider(_fixture);
        }

        public Fixture UsingFixture
        {
            get { return _fixture; }
        }

        public T Yields<T>(string requestName = null, object constraints = null)
        {
            return new YieldsActionProvider(_fixture).Yields<T>(requestName, constraints);
        }

        public object Yields(Type type, string requestName = null, object constraints = null)
        {
            return new YieldsActionProvider(_fixture).Yields(type, requestName, constraints);
        }

        private IWebElement FindByAuto(string selector)
        {
            if (selector.StartsWith("//"))
            {
                return _fixture.Driver.FindElement(By.XPath(selector));
            }

            var jqueryElements = FindAllByJQuery(selector, false);

            if (jqueryElements != null)
            {
                if (jqueryElements.Count == 0)
                {
                    throw new Exception("Could not locate element with jquery selector: " + selector);
                }

                return jqueryElements[0];
            }

            return _fixture.Driver.FindElement(By.CssSelector(selector));
        }

        private IWebElement FindByJQuery(string selector)
        {
            var elements = FindAllByJQuery(selector, true);

            if (elements.Count == 0)
            {
                throw new Exception("Could not find element element using jquery selector: " + selector);
            }
            return null;
        }

        private ReadOnlyCollection<IWebElement> FindAllByJQuery(string selector, bool throwIfNoJQuery)
        {
            bool jqueryFound = false;

            IJavaScriptExecutor executor = _fixture.Driver as IJavaScriptExecutor;

            if (executor != null)
            {
                jqueryFound = (bool)executor.ExecuteScript("return typeof window.jQuery == 'undefined'");
            }

            if (jqueryFound)
            {
                IEnumerable<object> matchedItems = (IEnumerable<object>)executor.ExecuteScript("return fluentjQuery.find('" + selector + "')");

                return new ReadOnlyCollection<IWebElement>(matchedItems.Cast<IWebElement>().ToList());
            }

            if (throwIfNoJQuery)
            {
                throw new Exception("window.jQuery not found");
            }

            return null;
        }

        private ReadOnlyCollection<IWebElement> FindAllByAuto(string selector)
        {
            if (selector.StartsWith("//"))
            {
                return _fixture.Driver.FindElements(By.XPath(selector));
            }

            var jqueryElements = FindAllByJQuery(selector, false);

            if (jqueryElements != null)
            {
                return jqueryElements;
            }

            return _fixture.Driver.FindElements(By.CssSelector(selector));
        }
    }
}
