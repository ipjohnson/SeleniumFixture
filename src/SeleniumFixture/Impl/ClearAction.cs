using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IClearAction
    {
        /// <summary>
        /// Clear by selector
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>action provider</returns>
        IActionProvider Clear(string selector);

        /// <summary>
        /// Clear by selenium selector
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>action provider</returns>
        IActionProvider Clear(By selector);
    }

    /// <summary>
    /// Clear action
    /// </summary>
    public class ClearAction : IClearAction
    {
        private IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public ClearAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Clear by selector
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>action provider</returns>
        public IActionProvider Clear(string selector)
        {
            _actionProvider.FindElements(selector).Apply(e => e.Clear());

            return _actionProvider;
        }

        /// <summary>
        /// Clear by selenium selector
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>action provider</returns>
        public IActionProvider Clear(By selector)
        {
            _actionProvider.FindElements(selector).Apply(e => e.Clear());

            return _actionProvider;
        }
    }
}
