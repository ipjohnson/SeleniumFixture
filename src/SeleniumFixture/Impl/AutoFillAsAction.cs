using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IAutoFillAsAction<T>
    {
        IThenSubmitAction PerformFill(string requestName, object constraints);
    }

    public class AutoFillAsAction<T> : IAutoFillAsAction<T>
    {
        protected readonly IActionProvider ActionProvider;
        protected readonly IEnumerable<IWebElement> Elements;

        public AutoFillAsAction(IActionProvider actionProvider, IEnumerable<IWebElement> elements)
        {
            ActionProvider = actionProvider;
            Elements = elements;
        }

        public virtual IThenSubmitAction PerformFill(string requestName, object constraints)
        {
            var seedValue = ActionProvider.UsingFixture.Data.Generate<T>(requestName, constraints);

            return new AutoFillAction(ActionProvider, Elements, seedValue).PerformFill();
        }
    }
}
