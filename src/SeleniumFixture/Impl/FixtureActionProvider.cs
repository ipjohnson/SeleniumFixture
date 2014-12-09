﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using SimpleFixture;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Action provider
    /// </summary>
    public class FixtureActionProvider : IActionProvider
    {
        protected readonly Fixture _fixture;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture"></param>
        public FixtureActionProvider(Fixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// Navigate the fixture
        /// </summary>
        public INavigateActionProvider Navigate
        {
            get { return new NavigateActionProvider(this); }
        }

        /// <summary>
        /// Find a specified element by selector
        /// </summary>
        /// <param name="selector">selector to use to locate element</param>
        /// <returns>element or throws an exception</returns>
        public IWebElement FindElement(string selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            switch (_fixture.Configuration.Selector)
            {
                case SelectorAlgorithm.JQuery:
                    return _fixture.Driver.FindElement(Using.JQuery(selector));

                case SelectorAlgorithm.CSS:
                    return _fixture.Driver.FindElement(By.CssSelector(selector));

                case SelectorAlgorithm.XPath:
                    return _fixture.Driver.FindElement(By.XPath(selector));

                case SelectorAlgorithm.Auto:
                    return _fixture.Driver.FindElement(Using.Auto(selector));

                default:
                    throw new Exception("Unknown SelectorAlgorithm " + _fixture.Configuration.Selector);

            }
        }

        /// <summary>
        /// Find a specified by selector
        /// </summary>
        /// <param name="selector">by selector</param>
        /// <returns>elements</returns>
        public IWebElement FindElement(By selector)
        {
            return _fixture.Driver.FindElement(selector);
        }

        /// <summary>
        /// Find All elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        public ReadOnlyCollection<IWebElement> FindElements(string selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            switch (_fixture.Configuration.Selector)
            {
                case SelectorAlgorithm.JQuery:
                    return _fixture.Driver.FindElements(Using.JQuery(selector));

                case SelectorAlgorithm.CSS:
                    return _fixture.Driver.FindElements(By.CssSelector(selector));

                case SelectorAlgorithm.XPath:
                    return _fixture.Driver.FindElements(By.XPath(selector));

                case SelectorAlgorithm.Auto:
                    return _fixture.Driver.FindElements(Using.Auto(selector));

                default:
                    throw new Exception("Unknown SelectorAlgorithm " + _fixture.Configuration.Selector);

            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By element)
        {
            return _fixture.Driver.FindElements(element);
        }

        public bool CheckForElement(string element)
        {
            return FindElements(element).Any();
        }

        public bool CheckForElement(By element)
        {
            return FindElements(element).Any();
        }

        public int Count(string selector)
        {
            return FindElements(selector).Count;
        }

        public int Count(By selector)
        {
            return FindElements(selector).Count;
        }

        public IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    FindElement(selector).Click();
                    break;
                case ClickMode.ClickAny:
                    FindElements(selector).Apply(c => c.Click());
                    break;
                case ClickMode.ClickAll:
                    var all = FindElements(selector);

                    if (all.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    all.Apply(c => c.Click());
                    break;
                case ClickMode.ClickFirst:
                    var firstList = FindElements(selector);

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
                    FindElement(selector).Click();
                    break;
                case ClickMode.ClickAny:
                    FindElements(selector).Apply(c => c.Click());
                    break;
                case ClickMode.ClickAll:
                    var all = FindElements(selector);

                    if (all.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    all.Apply(c => c.Click());
                    break;
                case ClickMode.ClickFirst:
                    var firstList = FindElements(selector);

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
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    {
                        var element = FindElement(selector);

                        Actions action = new Actions(_fixture.Driver);
                        action.DoubleClick(element);
                        action.Perform();
                    }
                    break;

                case ClickMode.ClickAny:
                    {
                        FindElements(selector).Apply(element =>
                                                {
                                                    Actions action = new Actions(_fixture.Driver);
                                                    action.DoubleClick(element);
                                                    action.Perform();
                                                });


                    }
                    break;
                case ClickMode.ClickAll:
                    {
                        var all = FindElements(selector);

                        if (all.Count == 0)
                        {
                            throw new Exception("Could not locate any using selector: " + selector);
                        }

                        all.Apply(element =>
                                    {
                                        Actions action = new Actions(_fixture.Driver);
                                        action.DoubleClick(element);
                                        action.Perform();
                                    });
                    }
                    break;

                case ClickMode.ClickFirst:
                    {
                        var firstList = FindElements(selector);

                        if (firstList.Count == 0)
                        {
                            throw new Exception("Could not locate any using selector: " + selector);
                        }

                        Actions action = new Actions(_fixture.Driver);
                        action.DoubleClick(firstList[0]);
                        action.Perform();
                    }
                    break;
            }

            return this;
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

        public IThenSubmitActionProvider AutoFillAs<T>(string selector, string requestName = null, object constraints = null)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFillAs<T>(By selector, string requestName = null, object constraints = null)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFillAs<T>(IEnumerable<IWebElement> elements, string requestName = null, object constraints = null)
        {
            throw new NotImplementedException();
        }

        public IFillActionProvider Fill(string selector)
        {
            return Fill(FindElements(selector));
        }

        public IFillActionProvider Fill(By selector)
        {
            return Fill(FindElements(selector));
        }

        public IFillActionProvider Fill(IEnumerable<IWebElement> elements)
        {
            ReadOnlyCollection<IWebElement> readOnlyElements = elements as ReadOnlyCollection<IWebElement> ??
                                                               new ReadOnlyCollection<IWebElement>(new List<IWebElement>(elements));

            return new FillActionProvider(readOnlyElements, _fixture);
        }

        public IWaitActionProvider Wait
        {
            get { return new WaitActionProvider(this); }
        }

        public IYieldsActionProvider Submit(string selector)
        {
            FindElement(selector).Submit();

            return new YieldsActionProvider(_fixture);
        }

        public IYieldsActionProvider Submit(By selector)
        {
            FindElement(selector).Submit();

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

        private IWebElement FindByAuto(string selector, IWebElement startingElement)
        {
            if (selector.StartsWith("//"))
            {
                return _fixture.Driver.FindElement(By.XPath(selector));
            }

            var jqueryElements = FindAllByJQuery(selector, startingElement, false);

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

        private IWebElement FindByJQuery(string selector, IWebElement startingElement)
        {
            var elements = FindAllByJQuery(selector, startingElement, true);

            if (elements.Count == 0)
            {
                throw new Exception("Could not find element element using jquery selector: " + selector);
            }

            return null;
        }

        private ReadOnlyCollection<IWebElement> FindAllByJQuery(string selector, IWebElement startingElement, bool throwIfNoJQuery)
        {
            bool jqueryFound = false;

            IJavaScriptExecutor executor = _fixture.Driver as IJavaScriptExecutor;

            if (executor != null)
            {
                jqueryFound = (bool)executor.ExecuteScript("return typeof window.jQuery == 'undefined'");
            }

            if (jqueryFound)
            {
                return startingElement != null ?
                       startingElement.FindElements(Using.JQuery(selector)) :
                       _fixture.Driver.FindElements(Using.JQuery(selector));
            }

            if (throwIfNoJQuery)
            {
                throw new Exception("window.jQuery not found");
            }

            return null;
        }

        private ReadOnlyCollection<IWebElement> FindAllByAuto(string selector, IWebElement startingElement)
        {
            if (selector.StartsWith("//"))
            {
                return _fixture.Driver.FindElements(By.XPath(selector));
            }

            var jqueryElements = FindAllByJQuery(selector, startingElement, false);

            if (jqueryElements != null)
            {
                return jqueryElements;
            }

            return _fixture.Driver.FindElements(By.CssSelector(selector));
        }
    }
}
