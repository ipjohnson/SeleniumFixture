using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IClickAction
    {
        IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll);

        IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll);
    }

    /// <summary>
    /// Click provider
    /// </summary>
    public class ClickAction : IClickAction
    {
        protected readonly IActionProvider _actionProvider;

        public ClickAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        public virtual IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    _actionProvider.FindElement(selector).Click();
                    break;

                case ClickMode.ClickAny:
                    _actionProvider.FindElements(selector).Apply(c => c.Click());
                    break;

                case ClickMode.ClickAll:
                    var all = _actionProvider.FindElements(selector);

                    if (all.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    all.Apply(c => c.Click());
                    break;

                case ClickMode.ClickFirst:
                    var firstList = _actionProvider.FindElements(selector);

                    if (firstList.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    firstList[0].Click();
                    break;
            }
            var configuration = _actionProvider.UsingFixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            return configuration.AlwaysWaitForAjax ? _actionProvider.Wait.ForAjax().Then : _actionProvider;
        }

        public virtual IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    _actionProvider.FindElement(selector).Click();
                    break;
                case ClickMode.ClickAny:
                    _actionProvider.FindElements(selector).Apply(c => c.Click());
                    break;
                case ClickMode.ClickAll:
                    var all = _actionProvider.FindElements(selector);

                    if (all.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    all.Apply(c => c.Click());
                    break;
                case ClickMode.ClickFirst:
                    var firstList = _actionProvider.FindElements(selector);

                    if (firstList.Count == 0)
                    {
                        throw new Exception("Could not locate any using selector: " + selector);
                    }
                    firstList[0].Click();
                    break;
            }

            var configuration = _actionProvider.UsingFixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }
            
            return configuration.AlwaysWaitForAjax ? _actionProvider.Wait.ForAjax().Then : _actionProvider;
        }
    }
}
