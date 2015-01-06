using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public SwitchAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Switch to a window by name
        /// </summary>
        /// <param name="windowName">window name</param>
        /// <returns></returns>
        public IYieldsAction Window(string windowName)
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Window(windowName);

            return new YieldsAction(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to parent frame
        /// </summary>
        /// <returns></returns>
        public IYieldsAction ParentFrame()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().ParentFrame();

            return new YieldsAction(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to frame by specific name
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        public IYieldsAction Frame(string frameName)
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Frame(frameName);

            return new YieldsAction(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to frame by element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IYieldsAction Frame(IWebElement element)
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Frame(element);

            return new YieldsAction(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to default content
        /// </summary>
        /// <returns></returns>
        public IYieldsAction DefaultContent()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().DefaultContent();

            return new YieldsAction(_actionProvider.UsingFixture);
        }

        /// <summary>
        /// Switch to alert
        /// </summary>
        /// <returns></returns>
        public IAlert Alert()
        {
            return _actionProvider.UsingFixture.Driver.SwitchTo().Alert();
        }
    }
}
