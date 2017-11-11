using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumFixture.Impl
{
    public interface IDoubleClickAction
    {
        IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll);

        IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll);

        IActionProvider DoubleClick(IEnumerable<IWebElement> elements, ClickMode clickMode = ClickMode.ClickAll);
    }

    public class DoubleClickAction : IDoubleClickAction
    {
        protected readonly IActionProvider _actionProvider;
        protected readonly Fixture _fixture;

        public DoubleClickAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
            _fixture = _actionProvider.UsingFixture;
        }

        public IActionProvider DoubleClick(IEnumerable<IWebElement> elements, ClickMode clickMode = ClickMode.ClickAll)
        {
            bool found = false;
            Actions action = null;

            switch (clickMode)
            {
                case ClickMode.ClickAll:
                    elements.Apply(allElement =>
                    {
                        action = new Actions(_fixture.Driver);
                        action.DoubleClick(allElement);
                        action.Perform();
                        found = true;
                    });

                    if(!found)
                    {
                        throw new Exception("Must provide elements to click");
                    }
                    break;
                case ClickMode.ClickAny:
                    elements.Apply(anyElement =>
                    {
                        action = new Actions(_fixture.Driver);
                        action.DoubleClick(anyElement);
                        action.Perform();
                        found = true;
                    });
                    break;
                case ClickMode.ClickFirst:
                    var firstElement = elements.First();

                    action = new Actions(_fixture.Driver);
                    action.DoubleClick(firstElement);
                    action.Perform();
                    break;
                case ClickMode.ClickOne:
                    var oneElement = elements.FirstOrDefault();
                    if(oneElement != null)

                    {
                        action = new Actions(_fixture.Driver);
                        action.DoubleClick(oneElement);
                        action.Perform();
                    }
                    break;
            }

            var configuration = _fixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            return configuration.AlwaysWaitForAjax ? _fixture.Wait.ForAjax().Then : _actionProvider;
        }

        public virtual IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    {
                        var element = _actionProvider.FindElement(selector);

                        Actions action = new Actions(_fixture.Driver);
                        action.DoubleClick(element);
                        action.Perform();
                    }
                    break;

                case ClickMode.ClickAny:
                    {
                        _actionProvider.FindElements(selector).Apply(element =>
                        {
                            Actions action = new Actions(_fixture.Driver);
                            action.DoubleClick(element);
                            action.Perform();
                        });
                    }
                    break;
                case ClickMode.ClickAll:
                    {
                        var all = _actionProvider.FindElements(selector);

                        if (all.Count == 0)
                        {
                            throw new Exception("Could not locate any using selector: " + selector);
                        }

                        all.Apply(element =>
                        {
                            Actions action = new Actions(_fixture.Driver);
                            action.DoubleClick(element);
                            action.Perform();
                        });
                    }
                    break;

                case ClickMode.ClickFirst:
                    {
                        var firstList = _actionProvider.FindElements(selector);

                        if (firstList.Count == 0)
                        {
                            throw new Exception("Could not locate any using selector: " + selector);
                        }

                        Actions action = new Actions(_fixture.Driver);
                        action.DoubleClick(firstList[0]);
                        action.Perform();
                    }
                    break;
            }

            var configuration = _fixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            return configuration.AlwaysWaitForAjax ? _fixture.Wait.ForAjax().Then : _actionProvider;
        }

        public virtual IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            switch (clickMode)
            {
                case ClickMode.ClickOne:
                    {
                        var element = _actionProvider.FindElement(selector);

                        Actions action = new Actions(_fixture.Driver);
                        action.DoubleClick(element);
                        action.Perform();
                    }
                    break;

                case ClickMode.ClickAny:
                    {
                        _actionProvider.FindElements(selector).Apply(element =>
                        {
                            Actions action = new Actions(_fixture.Driver);
                            action.DoubleClick(element);
                            action.Perform();
                        });


                    }
                    break;
                case ClickMode.ClickAll:
                    {
                        var all = _actionProvider.FindElements(selector);

                        if (all.Count == 0)
                        {
                            throw new Exception("Could not locate any using selector: " + selector);
                        }

                        all.Apply(element =>
                        {
                            Actions action = new Actions(_fixture.Driver);
                            action.DoubleClick(element);
                            action.Perform();
                        });
                    }
                    break;

                case ClickMode.ClickFirst:
                    {
                        var firstList = _actionProvider.FindElements(selector);

                        if (firstList.Count == 0)
                        {
                            throw new Exception("Could not locate any using selector: " + selector);
                        }

                        Actions action = new Actions(_fixture.Driver);
                        action.DoubleClick(firstList[0]);
                        action.Perform();
                    }
                    break;
            }

            var configuration = _fixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            return configuration.AlwaysWaitForAjax ? _fixture.Wait.ForAjax().Then : _actionProvider;
        }
    }
}
