using System.Collections.Generic;
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

        /// <summary>
        /// Clear all elements provided
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        IActionProvider Clear(IEnumerable<IWebElement> elements);
    }

    /// <summary>
    /// Clear action
    /// </summary>
    public class ClearAction : IClearAction
    {
        protected readonly IActionProvider ActionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public ClearAction(IActionProvider actionProvider)
        {
            ActionProvider = actionProvider;
        }

        /// <summary>
        /// Clear the elements provided
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Clear(IEnumerable<IWebElement> elements)
        {
            elements.Apply(e => e.Clear());

            return ActionProvider;
        }

        /// <summary>
        /// Clear by selector
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>action provider</returns>
        public virtual IActionProvider Clear(string selector)
        {
            ActionProvider.FindElements(selector).Apply(e => e.Clear());

            return ActionProvider;
        }

        /// <summary>
        /// Clear by selenium selector
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>action provider</returns>
        public virtual IActionProvider Clear(By selector)
        {
            ActionProvider.FindElements(selector).Apply(e => e.Clear());

            return ActionProvider;
        }
    }
}
