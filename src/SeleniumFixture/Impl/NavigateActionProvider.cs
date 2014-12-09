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
    public interface INavigateActionProvider
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

    public class NavigateActionProvider : INavigateActionProvider
    {
        private readonly IActionProvider _actionProvider;

        public NavigateActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        public IActionProvider To(string url = null)
        {
            if (url == null || !url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                url = _actionProvider.UsingFixture.Configuration.BaseAddress + url;
            }

            _actionProvider.UsingFixture.Driver.Navigate().GoToUrl(url);

            return _actionProvider;
        }

        public T To<T>(string url = null)
        {
            To(url);

            return _actionProvider.Yields<T>();
        }

        public IActionProvider To(Uri uri)
        {
            _actionProvider.UsingFixture.Driver.Navigate().GoToUrl(uri);

            return _actionProvider;
        }

        public T To<T>(Uri uri)
        {
            To(uri);

            return _actionProvider.Yields<T>();
        }

        public IActionProvider Back()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Back();

            return _actionProvider;
        }

        public T Back<T>()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Back();

            return _actionProvider.Yields<T>();
        }

        public IActionProvider Forward()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Forward();

            return _actionProvider;
        }

        public T Forward<T>()
        {
            _actionProvider.UsingFixture.Driver.Navigate().Forward();

            return _actionProvider.Yields<T>();
        }
    }
}
