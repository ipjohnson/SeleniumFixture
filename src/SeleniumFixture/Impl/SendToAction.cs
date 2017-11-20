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
        protected readonly string SendValue;
        protected readonly IActionProvider ActionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        /// <param name="sendValue">send value</param>
        public SendToAction(IActionProvider actionProvider, string sendValue)
        {
            SendValue = sendValue;
            ActionProvider = actionProvider;
        }

        /// <summary>
        /// Send value to alert
        /// </summary>
        /// <returns></returns>
        public virtual IActionProvider ToAlert()
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().Alert().SendKeys(SendValue);

            return ActionProvider;
        }

        /// <summary>
        /// Select a set of elements to send value to
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>action provider</returns>
        public virtual IActionProvider To(string selector)
        {
            ActionProvider.FindElements(selector).Apply(e => e.SendKeys(SendValue));

            return ActionProvider;
        }

        /// <summary>
        /// Select a set of elements to send value to 
        /// </summary>
        /// <param name="selector">by selector</param>
        /// <returns>action provider</returns>
        public virtual IActionProvider To(By selector)
        {
            ActionProvider.FindElements(selector).Apply(e => e.SendKeys(SendValue));

            return ActionProvider;
        }
    }
}
