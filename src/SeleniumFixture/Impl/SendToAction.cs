using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Send to action interface
    /// </summary>
    public interface ISendToAction
    {
        /// <summary>
        /// Send value to alert
        /// </summary>
        /// <returns></returns>
        IActionProvider ToAlert();

        /// <summary>
        /// Select a set of elements to send value to
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>action provider</returns>
        IActionProvider To(string selector);

        /// <summary>
        /// Select a set of elements to send value to 
        /// </summary>
        /// <param name="selector">by selector</param>
        /// <returns>action provider</returns>
        IActionProvider To(By selector);
    }

    /// <summary>
    /// Send to action
    /// </summary>
    public class SendToAction : ISendToAction
    {
        private readonly string _sendValue;
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        /// <param name="sendValue">send value</param>
        public SendToAction(IActionProvider actionProvider, string sendValue)
        {
            _sendValue = sendValue;
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Send value to alert
        /// </summary>
        /// <returns></returns>
        public IActionProvider ToAlert()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Alert().SendKeys(_sendValue);

            return _actionProvider;
        }

        /// <summary>
        /// Select a set of elements to send value to
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>action provider</returns>
        public IActionProvider To(string selector)
        {
            _actionProvider.FindElements(selector).Apply(e => e.SendKeys(_sendValue));

            return _actionProvider;
        }

        /// <summary>
        /// Select a set of elements to send value to 
        /// </summary>
        /// <param name="selector">by selector</param>
        /// <returns>action provider</returns>
        public IActionProvider To(By selector)
        {
            _actionProvider.FindElements(selector).Apply(e => e.SendKeys(_sendValue));

            return _actionProvider;
        }
    }
}
