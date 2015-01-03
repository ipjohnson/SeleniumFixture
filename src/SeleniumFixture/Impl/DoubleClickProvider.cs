using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumFixture.Impl
{
    public interface IDoubleClickProvider
    {
        IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll);

        IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll);
    }

    public class DoubleClickProvider : IDoubleClickProvider
    {
        private IActionProvider _actionProvider;
        private Fixture _fixture;

        public DoubleClickProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
            _fixture = _actionProvider.UsingFixture;
        }

        public IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll)
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

            return _actionProvider;
        }

        public IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll)
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

            return _actionProvider;
        }
    }
}
