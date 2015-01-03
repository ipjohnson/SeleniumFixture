using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Provides navigation actions
    /// </summary>
    public interface INavigateAction
    {
        /// <summary>
        /// Navigate to the specified url
        /// </summary>
        /// <param name="url">url, if null navigate to base address</param>
        /// <returns>action provider</returns>
        IActionProvider To(string url = null);

        /// <summary>
        /// Navigate to specified url and return page object
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <param name="url">url to navigate to</param>
        /// <returns>page object</returns>
        T To<T>(string url = null);

        /// <summary>
        /// Navigate to provided Uri
        /// </summary>
        /// <param name="uri">uri to navigate to</param>
        /// <returns></returns>
        IActionProvider To(Uri uri);

        /// <summary>
        /// Navigate to provided uri and return page object
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <param name="uri">uri</param>
        /// <returns>page object</returns>
        T To<T>(Uri uri);

        /// <summary>
        /// Navigate the browser back
        /// </summary>
        /// <returns></returns>
        IActionProvider Back();

        /// <summary>
        /// Navigate back and return page model
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <returns>page object</returns>
        T Back<T>();

        /// <summary>
        /// Navigate the browser forward
        /// </summary>
        /// <returns></returns>
        IActionProvider Forward();

        /// <summary>
        /// Navigate forward and return page object
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <returns></returns>
        T Forward<T>();
    }

    /// <summary>
    /// Provide fluent syntax for Navigation
    /// </summary>
    public class NavigateAction : INavigateAction
    {
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public NavigateAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Navigate to the specified url
        /// </summary>
        /// <param name="url">url, if null navigate to base address</param>
        /// <returns>action provider</returns>
        public IActionProvider To(string url = null)
        {
            if (url == null || !url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                url = _actionProvider.UsingFixture.Configuration.BaseAddress + url;
            }

            _actionProvider.UsingFixture.Driver.Navigate().GoToUrl(url);

            return _actionProvider;
        }

        /// <summary>
        /// Navigate to specified url and return page object
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <param name="url">url to navigate to</param>
        /// <returns>page object</returns>
        public T To<T>(string url = null)
        {
            To(url);

            return _actionProvider.Yields<T>();
        }

        /// <summary>
        /// Navigate to provided Uri
        /// </summary>
        /// <param name="uri">uri to navigate to</param>
        /// <returns></returns>
        public IActionProvider To(Uri uri)
        {
            _actionProvider.UsingFixture.Driver.Navigate().GoToUrl(uri);

            return _actionProvider;
        }

        /// <summary>
        /// Navigate to provided uri and return page object
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <param name="uri">uri</param>
        /// <returns>page object</returns>
        public T To<T>(Uri uri)
        {
            To(uri);

            return _actionProvider.Yields<T>();
        }

        /// <summary>
        /// Navigate the browser back
        /// </summary>
        /// <returns></returns>
        public IActionProvider Back()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Back();

            return _actionProvider;
        }

        /// <summary>
        /// Navigate back and return page model
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <returns>page object</returns>
        public T Back<T>()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Back();

            return _actionProvider.Yields<T>();
        }

        /// <summary>
        /// Navigate the browser forward
        /// </summary>
        /// <returns></returns>
        public IActionProvider Forward()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Forward();

            return _actionProvider;
        }

        /// <summary>
        /// Navigate forward and return page object
        /// </summary>
        /// <typeparam name="T">page object type</typeparam>
        /// <returns></returns>
        public T Forward<T>()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Forward();

            return _actionProvider.Yields<T>();
        }
    }
}
