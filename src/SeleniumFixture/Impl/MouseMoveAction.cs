using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumFixture.Impl
{
    public interface IMouseMoveAction
    {
        IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null);

        IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null);
    }

    public class MouseMoveAction : IMouseMoveAction
    {
        private IActionProvider _actionProvider;

        public MouseMoveAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        public IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null)
        {
            Actions action = new Actions(_actionProvider.UsingFixture.Driver);

            var element = _actionProvider.FindElement(selector);

            if (x.HasValue && y.HasValue)
            {
                action.MoveToElement(element, x.Value, y.Value);
            }
            else
            {
                action.MoveToElement(element);
            }

            action.Perform();

            return _actionProvider;
        }

        public IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null)
        {
            Actions action = new Actions(_actionProvider.UsingFixture.Driver);

            var element = _actionProvider.FindElement(selector);

            if (x.HasValue && y.HasValue)
            {
                action.MoveToElement(element, x.Value, y.Value);
            }
            else
            {
                action.MoveToElement(element);
            }

            action.Perform();

            return _actionProvider;
        }
    }
}
