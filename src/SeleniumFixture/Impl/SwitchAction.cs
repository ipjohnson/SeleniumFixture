using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface ISwitchToAction
    {
        /// <summary>
        /// Switch to a window by name
        /// </summary>
        /// <param name="windowName">window name</param>
        /// <returns></returns>
        IYieldsAction Window(string windowName);

        /// <summary>
        /// Switch to parent frame
        /// </summary>
        /// <returns></returns>
        IYieldsAction ParentFrame();

        /// <summary>
        /// Switch to frame by specific name
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        IYieldsAction Frame(string frameName);

        /// <summary>
        /// Switch to frame by element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        IYieldsAction Frame(IWebElement element);

        /// <summary>
        /// Switch to element using by
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        IYieldsAction Frame(By selector);

        /// <summary>
        /// Switch to default content
        /// </summary>
        /// <returns></returns>
        IYieldsAction DefaultContent();

        /// <summary>
        /// Switch to alert
        /// </summary>
        /// <returns></returns>
        IAlert Alert();
    }

    /// <summary>
    /// Switch action provider
    /// </summary>
    public class SwitchAction : ISwitchToAction
    {
        protected readonly IActionProvider ActionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public SwitchAction(IActionProvider actionProvider)
        {
            ActionProvider = actionProvider;
        }

        /// <summary>
        /// Switch to a window by name
        /// </summary>
        /// <param name="windowName">window name</param>
        /// <returns></returns>
        public virtual IYieldsAction Window(string windowName)
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().Window(windowName);

            return new YieldsAction(ActionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to parent frame
        /// </summary>
        /// <returns></returns>
        public virtual IYieldsAction ParentFrame()
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().ParentFrame();

            return new YieldsAction(ActionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to frame by specific name
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        public virtual IYieldsAction Frame(string frameName)
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().Frame(frameName);

            return new YieldsAction(ActionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to frame by element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual IYieldsAction Frame(IWebElement element)
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().Frame(element);

            return new YieldsAction(ActionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to element using by
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual IYieldsAction Frame(By selector)
        {
            var element = ActionProvider.FindElement(selector);

            ActionProvider.UsingFixture.Driver.SwitchTo().Frame(element);

            return new YieldsAction(ActionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to default content
        /// </summary>
        /// <returns></returns>
        public virtual IYieldsAction DefaultContent()
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().DefaultContent();

            return new YieldsAction(ActionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to alert
        /// </summary>
        /// <returns></returns>
        public virtual IAlert Alert()
        {
            return ActionProvider.UsingFixture.Driver.SwitchTo().Alert();
        }
    }
}
