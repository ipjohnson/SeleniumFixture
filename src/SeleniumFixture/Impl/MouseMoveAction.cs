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
        protected readonly IActionProvider ActionProvider;

        public MouseMoveAction(IActionProvider actionProvider)
        {
            ActionProvider = actionProvider;
        }

        public virtual IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null)
        {
            var action = new Actions(ActionProvider.UsingFixture.Driver);

            var element = ActionProvider.FindElement(selector);

            if (x.HasValue && y.HasValue)
            {
                action.MoveToElement(element, x.Value, y.Value);
            }
            else
            {
                action.MoveToElement(element);
            }

            action.Perform();

            return ActionProvider;
        }

        public virtual IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null)
        {
            var action = new Actions(ActionProvider.UsingFixture.Driver);

            var element = ActionProvider.FindElement(selector);

            if (x.HasValue && y.HasValue)
            {
                action.MoveToElement(element, x.Value, y.Value);
            }
            else
            {
                action.MoveToElement(element);
            }

            action.Perform();

            return ActionProvider;
        }
    }
}
