using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        /// Move the mouse to a give element or x,y
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="x">x offset</param>
        /// <param name="y">y offset</param>
        /// <returns></returns>
        public IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null)
        {
            return _fixture.Data.Locate<IMouseMoveAction>().MoveTheMouseTo(selector, x, y);
        }

        /// <summary>
        /// Move the mouse to a give element or x,y
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="x">x offset</param>
        /// <param name="y">y offset</param>
        /// <returns></returns>
        public IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null)
        {
            return _fixture.Data.Locate<IMouseMoveAction>().MoveTheMouseTo(selector, x, y);
        }

        /// <summary>
        /// Navigate the fixture
        /// </summary>
        public INavigateAction Navigate
        {
            get { return _fixture.Data.Locate<INavigateAction>(); }
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

        /// <summary>
        /// Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context 
        ///             using the given mechanism.
        /// </summary>
        /// <param name="element">The locating mechanism to use.</param>
        /// <returns>
        /// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
        ///             matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<IWebElement> FindElements(By element)
        {
            return _fixture.Driver.FindElements(element);
        }

        /// <summary>
        /// Generate randome data
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="constraints"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Generate<T>(string requestName = null, object constraints = null)
        {
            return _fixture.Data.Generate<T>(requestName, constraints);
        }

        /// <summary>
        /// Get values from page
        /// </summary>
        public IGetAction Get
        {
            get { return _fixture.Data.Locate<IGetAction>(); }
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
            return _fixture.Data.Locate<IClickAction>().Click(selector, clickMode);
        }

        public IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IClickAction>().Click(selector, clickMode);
        }

        public IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IDoubleClickAction>().DoubleClick(selector, clickMode);
        }

        public IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IDoubleClickAction>().DoubleClick(selector, clickMode);
        }

        public T ExecuteJavaScript<T>(string javascript, params object[] args)
        {
            IJavaScriptExecutor executor = _fixture.Driver as IJavaScriptExecutor;

            if (executor == null)
            {
                throw new Exception("Javascript not supported by driver " + _fixture.Driver);
            }

            object returnValue = executor.ExecuteScript(javascript, args);

            if (typeof(T) == typeof(IEnumerable<IWebElement>) ||
                typeof(T) == typeof(ICollection<IWebElement>) ||
                typeof(T) == typeof(IList<IWebElement>) ||
                typeof(T) == typeof(List<IWebElement>))
            {
                var objects = (IEnumerable<object>)returnValue;

                returnValue = objects.Cast<IWebElement>().ToList();
            }
            else if (typeof(T) == typeof(ReadOnlyCollection<IWebElement>) ||
                     typeof(T) == typeof(IReadOnlyList<IWebElement>) ||
                     typeof(T) == typeof(IReadOnlyCollection<IWebElement>))
            {
                var objects = (IEnumerable<object>)returnValue;

                returnValue = new ReadOnlyCollection<IWebElement>(objects.Cast<IWebElement>().ToList());
            }
            else if (returnValue.GetType().IsInstanceOfType(typeof(T)))
            {
                returnValue = Convert.ChangeType(returnValue, typeof(T));
            }

            return (T)returnValue;
        }

        public void ExecuteJavaScript(string javascript, params object[] args)
        {
            IJavaScriptExecutor executor = _fixture.Driver as IJavaScriptExecutor;

            if (executor == null)
            {
                throw new Exception("Javascript not supported by driver " + _fixture.Driver);
            }

            executor.ExecuteScript(javascript, args);
        }

        public IThenSubmitAction AutoFill(string selector, object seedWith = null)
        {
            return AutoFill(FindElements(selector), seedWith);
        }

        public IThenSubmitAction AutoFill(By selector, object seedWith = null)
        {
            return AutoFill(FindElements(selector), seedWith);
        }

        public IThenSubmitAction AutoFill(IEnumerable<IWebElement> elements, object seedWith = null)
        {
            return _fixture.Data.Locate<IAutoFillAction>(constraints: new { elements, seedWith }).PerformFill();
        }

        public IThenSubmitAction AutoFillAs<T>(string selector, string requestName = null, object constraints = null)
        {
            return AutoFillAs<T>(FindElements(selector), requestName, constraints);
        }

        public IThenSubmitAction AutoFillAs<T>(By selector, string requestName = null, object constraints = null)
        {
            return AutoFillAs<T>(FindElements(selector), requestName, constraints);
        }

        public IThenSubmitAction AutoFillAs<T>(IEnumerable<IWebElement> elements, string requestName = null, object constraints = null)
        {
            var autoFillProvider = _fixture.Data.Locate<IAutoFillAsActionProvider>();

            return autoFillProvider.CreateAction<T>(elements).PerformFill(requestName, constraints);
        }

        public IFillAction Fill(string selector)
        {
            return Fill(FindElements(selector));
        }

        public IFillAction Fill(By selector)
        {
            return Fill(FindElements(selector));
        }

        public IFillAction Fill(IEnumerable<IWebElement> elements)
        {
            ReadOnlyCollection<IWebElement> readOnlyElements = elements as ReadOnlyCollection<IWebElement> ??
                                                               new ReadOnlyCollection<IWebElement>(new List<IWebElement>(elements));

            return _fixture.Data.Locate<IFillAction>(constraints: new { elements = readOnlyElements });
        }

        public IWaitAction Wait
        {
            get { return _fixture.Data.Locate<IWaitAction>(); }
        }

        public IYieldsAction Submit(string selector)
        {
            FindElement(selector).Submit();

            return _fixture.Data.Locate<IYieldsAction>();
        }

        public IYieldsAction Submit(By selector)
        {
            FindElement(selector).Submit();

            return _fixture.Data.Locate<IYieldsAction>();
        }

        public ISwitchToAction SwitchTo
        {
            get { return new SwitchAction(this); }
        }

        public Fixture UsingFixture
        {
            get { return _fixture; }
        }

        public T Yields<T>(string requestName = null, object constraints = null)
        {
            return _fixture.Data.Locate<IYieldsAction>().Yields<T>(requestName, constraints);
        }

        public object Yields(Type type, string requestName = null, object constraints = null)
        {
            return _fixture.Data.Locate<IYieldsAction>().Yields(type, requestName, constraints);
        }

    }
}
