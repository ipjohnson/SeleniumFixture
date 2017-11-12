using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

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
        public virtual IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null)
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
        public virtual IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null)
        {
            return _fixture.Data.Locate<IMouseMoveAction>().MoveTheMouseTo(selector, x, y);
        }

        /// <summary>
        /// Navigate the fixture
        /// </summary>
        public virtual INavigateAction Navigate
        {
            get { return _fixture.Data.Locate<INavigateAction>(); }
        }

        /// <summary>
        /// Find a specified element by selector
        /// </summary>
        /// <param name="selector">selector to use to locate element</param>
        /// <returns>element or throws an exception</returns>
        public virtual IWebElement FindElement(string selector)
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
        public virtual IWebElement FindElement(By selector)
        {
            return _fixture.Driver.FindElement(selector);
        }

        /// <summary>
        /// Find All elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        public virtual ReadOnlyCollection<IWebElement> FindElements(string selector)
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
        /// Focus an element
        /// </summary>
        /// <param name="selector"></param>
        public virtual void Focus(string selector)
        {
            var element = FindElement(selector);

            ExecuteJavaScript("arguments[0].focus();", element);
        }

        /// <summary>
        /// Focus an element
        /// </summary>
        /// <param name="selector"></param>
        public virtual void Focus(By selector)
        {
            var element = FindElement(selector);

            ExecuteJavaScript("arguments[0].focus();", element);
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
        public virtual ReadOnlyCollection<IWebElement> FindElements(By element)
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
        public virtual T Generate<T>(string requestName = null, object constraints = null)
        {
            return _fixture.Data.Generate<T>(requestName, constraints);
        }

        /// <summary>
        /// Get values from page
        /// </summary>
        public virtual IGetAction Get
        {
            get { return _fixture.Data.Locate<IGetAction>(); }
        }

        /// <summary>
        /// Checks for element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public virtual bool CheckForElement(string element)
        {
            return FindElements(element).Any();
        }

        /// <summary>
        /// Checks for element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public virtual bool CheckForElement(By element)
        {
            return FindElements(element).Any();
        }

        /// <summary>
        /// Clear elements specified
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        public virtual IActionProvider Clear(string selector)
        {
            return _fixture.Data.Locate<IClearAction>().Clear(selector);
        }

        /// <summary>
        /// Clear elements specified
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        public virtual IActionProvider Clear(By selector)
        {
            return _fixture.Data.Locate<IClearAction>().Clear(selector);
        }

        /// <summary>
        /// Count the number of elements present
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>
        /// count of elements
        /// </returns>
        public virtual int Count(string selector)
        {
            return FindElements(selector).Count;
        }

        /// <summary>
        /// Count the number of elements present
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>
        /// count of elements
        /// </returns>
        public virtual int Count(By selector)
        {
            return FindElements(selector).Count;
        }

        /// <summary>
        /// SwitchTo Alert and dismiss
        /// </summary>
        /// <returns></returns>
        public virtual IActionProvider DismissAlert()
        {
            return _fixture.Data.Locate<IAlertAction>().Dismiss();
        }

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IClickAction>().Click(selector, clickMode);
        }

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IClickAction>().Click(selector, clickMode);
        }

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IDoubleClickAction>().DoubleClick(selector, clickMode);
        }

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _fixture.Data.Locate<IDoubleClickAction>().DoubleClick(selector, clickMode);
        }

        /// <summary>
        /// Execute arbitrary javascript
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="javascript"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Javascript not supported by driver  + _fixture.Driver</exception>
        public virtual T ExecuteJavaScript<T>(string javascript, params object[] args)
        {
            if (!(_fixture.Driver is IJavaScriptExecutor executor))
            {
                throw new Exception("Javascript not supported by driver " + _fixture.Driver);
            }

            var returnValue = executor.ExecuteScript(javascript, args);

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

        /// <summary>
        /// Execute arbitrary javascript
        /// </summary>
        /// <param name="javascript"></param>
        /// <param name="args"></param>
        /// <exception cref="System.Exception">Javascript not supported by driver  + _fixture.Driver</exception>
        public virtual void ExecuteJavaScript(string javascript, params object[] args)
        {
            if (!(_fixture.Driver is IJavaScriptExecutor executor))
            {
                throw new Exception("Javascript not supported by driver " + _fixture.Driver);
            }

            executor.ExecuteScript(javascript, args);
        }

        /// <summary>
        /// SwitchTo alert and accept.
        /// </summary>
        /// <returns></returns>
        public virtual IActionProvider AcceptAlert()
        {
            return _fixture.Data.Locate<IAlertAction>().Accept();
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IThenSubmitAction AutoFill(string selector, object seedWith = null)
        {
            return AutoFill(FindElements(selector), seedWith);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IThenSubmitAction AutoFill(By selector, object seedWith = null)
        {
            return AutoFill(FindElements(selector), seedWith);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="seedWith"></param>
        /// <returns></returns>
        public virtual IThenSubmitAction AutoFill(IEnumerable<IWebElement> elements, object seedWith = null)
        {
            return _fixture.Data.Locate<IAutoFillAction>(constraints: new { elements, seedWith }).PerformFill();
        }

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="selector">selector for elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IThenSubmitAction AutoFillAs<T>(string selector, string requestName = null, object constraints = null)
        {
            return AutoFillAs<T>(FindElements(selector), requestName, constraints);
        }

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="selector">selector for elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IThenSubmitAction AutoFillAs<T>(By selector, string requestName = null, object constraints = null)
        {
            return AutoFillAs<T>(FindElements(selector), requestName, constraints);
        }

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="elements">elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>
        /// this
        /// </returns>
        public virtual IThenSubmitAction AutoFillAs<T>(IEnumerable<IWebElement> elements, string requestName = null, object constraints = null)
        {
            var autoFillProvider = _fixture.Data.Locate<IAutoFillAsActionProvider>();

            return autoFillProvider.CreateAction<T>(elements).PerformFill(requestName, constraints);
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>
        /// fill action
        /// </returns>
        public virtual IFillAction Fill(string selector)
        {
            return Fill(FindElements(selector));
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>
        /// fill action
        /// </returns>
        public virtual IFillAction Fill(By selector)
        {
            return Fill(FindElements(selector));
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns>
        /// fill action
        /// </returns>
        public virtual IFillAction Fill(IEnumerable<IWebElement> elements)
        {
            var readOnlyElements = elements as ReadOnlyCollection<IWebElement> ??
                                                               new ReadOnlyCollection<IWebElement>(new List<IWebElement>(elements));

            return _fixture.Data.Locate<IFillAction>(constraints: new { elements = readOnlyElements });
        }

        /// <summary>
        /// Wait for something to happen
        /// </summary>
        public virtual IWaitAction Wait
        {
            get { return _fixture.Data.Locate<IWaitAction>(); }
        }

        /// <summary>
        /// Send the value to a particular element or set of elements
        /// </summary>
        /// <param name="sendValue">value to send to elements</param>
        /// <returns></returns>
        public virtual ISendToAction Send(object sendValue)
        {
            var sendValueString = sendValue.ToString();

            return _fixture.Data.Locate<ISendToAction>(constraints: new { sendValue = sendValueString });
        }

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual IYieldsAction Submit(string selector)
        {
            FindElement(selector).Submit();

            var configuration = UsingFixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            if (configuration.AlwaysWaitForAjax)
            {
                Wait.ForAjax();
            }

            return _fixture.Data.Locate<IYieldsAction>();
        }

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        public virtual IYieldsAction Submit(By selector)
        {
            FindElement(selector).Submit();

            var configuration = UsingFixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            if (configuration.AlwaysWaitForAjax)
            {
                Wait.ForAjax();
            }

            return _fixture.Data.Locate<IYieldsAction>();
        }

        /// <summary>
        /// Switch to
        /// </summary>
        public virtual ISwitchToAction SwitchTo
        {
            get { return new SwitchAction(this); }
        }

        /// <summary>
        /// Take a screen shot using the current driver.
        /// Note: some drivers do not support taking screen shots
        /// </summary>
        /// <param name="screenshotName">take screenshot, if null then ClassName_MethodName is used</param>
        /// <param name="throwsIfNotSupported">throw exception if screen shot is not supported by the current driver</param>
        /// <param name="format">Image format, png by default</param>
        /// <returns></returns>
        public virtual IActionProvider TakeScreenshot(string screenshotName = null, bool throwsIfNotSupported = false, ScreenshotImageFormat format = ScreenshotImageFormat.Png)
        {
            return _fixture.Data.Locate<ITakeScreenshotAction>().TakeScreenshot(screenshotName, throwsIfNotSupported, format);
        }

        /// <summary>
        /// Fixture for this action provider
        /// </summary>
        public virtual Fixture UsingFixture
        {
            get { return _fixture; }
        }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <typeparam name="T">Type of object to Generate</typeparam>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>
        /// new T
        /// </returns>
        public virtual T Yields<T>(string requestName = null, object constraints = null)
        {
            return _fixture.Data.Locate<IYieldsAction>().Yields<T>(requestName, constraints);
        }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <param name="type">Type of object to Generate</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>
        /// new instance
        /// </returns>
        public virtual object Yields(Type type, string requestName = null, object constraints = null)
        {
            return _fixture.Data.Locate<IYieldsAction>().Yields(type, requestName, constraints);
        }

        /// <summary>
        /// AutoFill provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IThenSubmitAction AutoFill(params IWebElement[] elements)
        {
            return AutoFill((IEnumerable<IWebElement>)elements);
        }

        /// <summary>
        /// AutoFill provided elements as a specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IThenSubmitAction AutoFillAs<T>(params IWebElement[] elements)
        {
            return AutoFill((IEnumerable<IWebElement>)elements);
        }

        /// <summary>
        /// Clear the provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Clear(IEnumerable<IWebElement> elements)
        {
            return _fixture.Data.Locate<IClearAction>().Clear(elements);
        }

        /// <summary>
        /// Clear the provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Clear(params IWebElement[] elements)
        {
            return _fixture.Data.Locate<IClearAction>().Clear(elements);
        }

        /// <summary>
        /// Click provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="clickMode"></param>
        /// <returns></returns>
        public IActionProvider Click(IEnumerable<IWebDriver> elements, ClickMode clickMode = ClickMode.ClickOne)
        {
            return _fixture.Data.Locate<IClickAction>().Click((IEnumerable<IWebElement>)elements, clickMode);
        }

        /// <summary>
        /// Click the provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Click(params IWebElement[] elements)
        {
            return Click((IEnumerable<IWebDriver>)elements, ClickMode.ClickAny);
        }

        /// <summary>
        /// Double click provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="clickMode"></param>
        /// <returns></returns>
        public IActionProvider DoubleClick(IEnumerable<IWebElement> elements, ClickMode clickMode = ClickMode.ClickOne)
        {
            return _fixture.Data.Locate<IDoubleClickAction>().DoubleClick(elements, clickMode);
        }

        /// <summary>
        /// Double click provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider DoubleClick(params IWebElement[] elements)
        {
            return DoubleClick(elements, ClickMode.ClickAny);
        }
        
        /// <summary>
        /// Fill provided elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IFillAction Fill(params IWebElement[] elements)
        {
            return Fill((IEnumerable<IWebElement>)elements);
        }
    }
}
