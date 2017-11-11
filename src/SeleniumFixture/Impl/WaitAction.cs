using System;
using System.Threading;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{

    /// <summary>
    /// Provides wait fluent syntax
    /// </summary>
    public interface IWaitAction
    {
        /// <summary>
        /// Wait for X number of seconds.
        /// </summary>
        /// <param name="seconds">wait time in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction For(double seconds);

        /// <summary>
        /// Wait for no ajax calls to be active
        /// </summary>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForAjax(double? timeout = null);

        /// <summary>
        /// Wait for an element to be visible
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForElement(string selector, double? timeout = null);

        /// <summary>
        /// Wait for an element to be visible
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForElement(By selector, double? timeout = null);

        /// <summary>
        /// Wait for page source to contain a specific string
        /// </summary>
        /// <param name="stringToCheckFor">string to check for in page source</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForPageSourceToContain(string stringToCheckFor, double? timeout = null);

        /// <summary>
        /// Wait for page source to pass a specific test
        /// </summary>
        /// <param name="matchFunc">function to test for</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForPageSource(Func<string,bool> matchFunc, double? timeout = null);

        /// <summary>
        /// Wait for a page title
        /// </summary>
        /// <param name="pageTitle">page title</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForPageTitle(string pageTitle, double? timeout = null);

        /// <summary>
        /// Wait for a page title by specified func
        /// </summary>
        /// <param name="pageTitleFunc">function to test against the page title</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction ForPageTitle(Func<string, bool> pageTitleFunc,double? timeout = null);

        /// <summary>
        /// Wait until the provided delegate is true
        /// </summary>
        /// <param name="testFunc">func to test</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitAction Until(Func<IActionProvider, bool> testFunc, double? timeout = null);

        /// <summary>
        /// Provides a way back to action syntax
        /// </summary>
        IActionProvider Then { get; }
    }

    /// <summary>
    /// Provides wait fluent syntax
    /// </summary>
    public class WaitAction : IWaitAction
    {
        protected readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider"></param>
        public WaitAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
            Then = _actionProvider;
        }

        /// <summary>
        /// Wait for X number of seconds.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public virtual IWaitAction For(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));

            return this;
        }

        /// <summary>
        /// Wait for no ajax calls to be active
        /// </summary>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public virtual IWaitAction ForAjax(double? timeout = null)
        {
            return Until(
                i =>
                {
                    // static cast because I want an exception to be thrown when the driver doesn't support executing javascript
                    var executor = (IJavaScriptExecutor)_actionProvider.UsingFixture.Driver;

                    return (bool)executor.ExecuteScript(_actionProvider.UsingFixture.Configuration.AjaxActiveTest);
                }, timeout)
                .For(0.1);
        }

        /// <summary>
        /// Wait for an element to be present
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public virtual IWaitAction ForElement(string selector, double? timeout = null)
        {
            return Until(i => i.CheckForElement(selector), timeout);
        }

        /// <summary>
        /// Wait for an element to be present
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public virtual IWaitAction ForElement(By selector, double? timeout = null)
        {
            return Until(i => i.CheckForElement(selector), timeout);
        }

        /// <summary>
        /// Wait for page source to contain a specific string
        /// </summary>
        /// <param name="stringToCheckFor">string to check for in page source</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public IWaitAction ForPageSourceToContain(string stringToCheckFor, double? timeout = null)
        {
            return Until(i => i.Get.PageSource.Contains(stringToCheckFor), timeout);
        }

        /// <summary>
        /// Wait for page source to contain a specific string
        /// </summary>
        /// <param name="matchFunc">function to test the page source</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public IWaitAction ForPageSource(Func<string, bool> matchFunc, double? timeout = null)
        {
            return Until(i => matchFunc(i.Get.PageSource), timeout);
        }

        /// <summary>
        /// Wait for a page title
        /// </summary>
        /// <param name="pageTitle"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual IWaitAction ForPageTitle(string pageTitle, double? timeout = null)
        {
            return ForPageTitle(s => s.Equals(pageTitle));
        }

        /// <summary>
        /// Wait for a page title by specified func
        /// </summary>
        /// <param name="pageTitleFunc"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual IWaitAction ForPageTitle(Func<string, bool> pageTitleFunc, double? timeout = null)
        {
            return Until(i => pageTitleFunc(i.Get.PageTitle), timeout);
        }

        /// <summary>
        /// Wait until the provided delegate is true
        /// </summary>
        /// <param name="testFunc">func to test</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public virtual IWaitAction Until(Func<IActionProvider, bool> testFunc, double? timeout = null)
        {
            if (!timeout.HasValue)
            {
                timeout = _actionProvider.UsingFixture.Configuration.DefaultTimeout;
            }

            var expire = DateTime.Now.AddSeconds(timeout.Value);
            var untilResult = false;

            var defaultWaitIntercal = (int)(_actionProvider.UsingFixture.Configuration.DefaultWaitInterval * 1000);

            while (!untilResult)
            {
                Thread.Sleep(defaultWaitIntercal);

                untilResult = testFunc(_actionProvider);

                if (DateTime.Now > expire)
                {
                    break;
                }
            }

            if (!untilResult)
            {
                throw new Exception("Timedout waiting: " + timeout);    
            }

            return this;
        }

        /// <summary>
        /// Provides a way back to action syntax
        /// </summary>
        public virtual IActionProvider Then { get; private set; }
    }
}
