using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface ISwitchToActionProvider
    {
        /// <summary>
        /// Switch to a window by name
        /// </summary>
        /// <param name="windowName">window name</param>
        /// <returns></returns>
        IYieldsActionProvider Window(string windowName);

        /// <summary>
        /// Switch to parent frame
        /// </summary>
        /// <returns></returns>
        IYieldsActionProvider ParentFrame();

        /// <summary>
        /// Switch to frame by specific name
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        IYieldsActionProvider Frame(string frameName);

        /// <summary>
        /// Switch to frame by element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        IYieldsActionProvider Frame(IWebElement element);

        /// <summary>
        /// Switch to default content
        /// </summary>
        /// <returns></returns>
        IYieldsActionProvider DefaultContent();

        /// <summary>
        /// Switch to alert
        /// </summary>
        /// <returns></returns>
        IYieldsActionProvider Alert();
    }

    /// <summary>
    /// Switch action provider
    /// </summary>
    public class SwitchActionProvider : ISwitchToActionProvider
    {
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public SwitchActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Switch to a window by name
        /// </summary>
        /// <param name="windowName">window name</param>
        /// <returns></returns>
        public IYieldsActionProvider Window(string windowName)
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Window(windowName);

            return new YieldsActionProvider(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to parent frame
        /// </summary>
        /// <returns></returns>
        public IYieldsActionProvider ParentFrame()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().ParentFrame();

            return new YieldsActionProvider(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to frame by specific name
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        public IYieldsActionProvider Frame(string frameName)
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Frame(frameName);

            return new YieldsActionProvider(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to frame by element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IYieldsActionProvider Frame(IWebElement element)
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Frame(element);

            return new YieldsActionProvider(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to default content
        /// </summary>
        /// <returns></returns>
        public IYieldsActionProvider DefaultContent()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().DefaultContent();

            return new YieldsActionProvider(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to alert
        /// </summary>
        /// <returns></returns>
        public IYieldsActionProvider Alert()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Alert();

            return new YieldsActionProvider(_actionProvider.UsingFixture);
        }
    }
}
